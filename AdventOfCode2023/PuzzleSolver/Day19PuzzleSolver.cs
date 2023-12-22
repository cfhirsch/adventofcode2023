using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day19PuzzleSolver : IPuzzleSolver
    {
        private static Regex compReg = new Regex(@"(\w+)([<>])(\d+):(\w+)", RegexOptions.Compiled);

        public string SolvePartOne(bool test = false)
        {
            var workflows = new Dictionary<string, string>();
            var ratings = new List<string>();

            bool doneWithWorkflows = false;
            foreach (string line in PuzzleReader.ReadLines(19, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    doneWithWorkflows = true;
                    continue;
                }

                if (!doneWithWorkflows)
                {
                    int bracketPos = line.IndexOf("{");
                    workflows.Add(line.Substring(0, bracketPos),
                                  line.Substring(bracketPos + 1, line.Length - bracketPos - 2));
                }
                else
                {
                    ratings.Add(line.Substring(1, line.Length - 2));
                }
            }

            long sum = 0;
            foreach (string rating in ratings)
            {
                var ratingsDict = rating.Split(',').Select(x => x.Split('=')).Select(
                    x => new KeyValuePair<string, int>(x[0], Int32.Parse(x[1]))).ToDictionary(
                        kvp => kvp.Key, kvp => kvp.Value);

                string workflowKey = "in";
                bool done = false;
                while (!done)
                {
                    string workflow = workflows[workflowKey];
                    string[] rules = workflow.Split(',');

                    foreach (string rule in rules)
                    {
                        Match match = compReg.Match(rule);
                        bool satisfiesRule = false;
                        if (match.Success)
                        {
                            string category = match.Groups[1].Value;
                            string comp = match.Groups[2].Value;
                            int val = Int32.Parse(match.Groups[3].Value);
                            string label = match.Groups[4].Value;

                            switch (comp)
                            {
                                case "<":
                                    satisfiesRule = ratingsDict[category] < val;
                                    break;

                                case ">":
                                    satisfiesRule = ratingsDict[category] > val;
                                    break;

                                default:
                                    throw new ArgumentException($"Unexpected comp value {comp}.");
                            }

                            if (satisfiesRule)
                            {
                                switch (label)
                                {
                                    case "A":
                                        sum += ratingsDict.Values.Sum();
                                        done = true;
                                        break;

                                    case "R":
                                        done = true;
                                        break;

                                    default:
                                        workflowKey = label;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            switch (rule)
                            {
                                case "A":
                                    sum += ratingsDict.Values.Sum();
                                    done = true;
                                    break;

                                case "R":
                                    done = true;
                                    break;

                                default:
                                    workflowKey = rule;
                                    break;
                            }
                        }

                        if (satisfiesRule)
                        {
                            break;
                        }
                    }
                }
            }

            return sum.ToString();
        }

        // Solution to this part cribbed from https://github.com/DanaL/AdventOfCode/blob/main/2023/Day19/Program.cs.
        public string SolvePartTwo(bool test = false)
        {
            var rules = new Dictionary<string, WorkFlowStep[]>();
            foreach (string line in PuzzleReader.ReadLines(19, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                (string name, WorkFlowStep[] steps) = ParseRule(line);

                rules.Add(name, steps);
            }

            ulong result = PartTwo(rules);
            return result.ToString();
        }

        internal static Field ToField(string ch)
        {
            switch (ch)
            {
                case "x":
                    return Field.X;

                case "m":
                    return Field.M;

                case "a":
                    return Field.A;

                case "s":
                    return Field.S;

                default:
                    throw new Exception("Hmm this shouldn't happen");
            };
        }

        static Dictionary<Field, int> ParsePart(string line)
        {
            var m = Regex.Match(line, @"{x=(\d+),m=(\d+),a=(\d+),s=(\d+)}");
            return new Dictionary<Field, int>
            {
                { Field.X, int.Parse(m.Groups[1].Value) }, { Field.M, int.Parse(m.Groups[2].Value) },
                { Field.A, int.Parse(m.Groups[3].Value) }, { Field.S, int.Parse(m.Groups[4].Value) }
            };
        }

        private static (string, WorkFlowStep[]) ParseRule(string line)
        {
            var steps = new List<WorkFlowStep>();

            int bracketPos = line.IndexOf("{");
            (string name, string ruleStr) = (line.Substring(0, bracketPos),
                                            line.Substring(bracketPos + 1, line.Length - bracketPos - 2));

            string[] rules = ruleStr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string rule in rules)
            {
                var match = compReg.Match(rule);
                if (match.Success)
                {
                    Field f = ToField(match.Groups[1].Value);
                    int v = Int32.Parse(match.Groups[3].Value);

                    switch (match.Groups[2].Value)
                    {
                        case "<":
                            steps.Add(new WorkFlowStep(f, match.Groups[4].Value, Op.LT, v));
                            break;

                        case ">":
                            steps.Add(new WorkFlowStep(f, match.Groups[4].Value, Op.GT, v));
                            break;

                        default:
                            throw new ArgumentException($"Unexpected op.");
                    }
                }
                else
                {
                    steps.Add(new WorkFlowStep(Field.Any, rule, Op.Pipe, -1));
                }
            }

            return (name, steps.ToArray());
        }

        static string Classify(Dictionary<Field, int> part, WorkFlowStep[] steps, int s)
        {
            switch (steps[s].Op)
            {
                case Op.LT:
                    return part[steps[s].In] < steps[s].Val ? steps[s].Out : Classify(part, steps, s + 1);

                case Op.GT:
                    return part[steps[s].In] > steps[s].Val ? steps[s].Out : Classify(part, steps, s + 1);

                case Op.Pipe:
                    return steps[s].Out;

                default:
                    throw new Exception("Hmm this shouldn't happen");
            };
        }

        private static List<(Dictionary<Field, PRange>, string, int)> SplitRange(Dictionary<Field, PRange> parts, WorkFlowStep step, string stepName, int stepNum)
        {
            var result = new List<(Dictionary<Field, PRange> parts, string, int)>();
            var pass = parts.ToDictionary(e => e.Key, e => e.Value);
            var fail = parts.ToDictionary(e => e.Key, e => e.Value);
            int lo, hi;

            switch (step.Op)
            {
                case Op.LT:
                    lo = parts[step.In].Lo;
                    hi = parts[step.In].Hi;
                    pass[step.In] = new PRange(lo, step.Val - 1);
                    fail[step.In] = new PRange(step.Val, hi);
                    result.Add((pass, step.Out, 0));
                    result.Add((fail, stepName, stepNum + 1));
                    break;
                case Op.GT:
                    lo = parts[step.In].Lo;
                    hi = parts[step.In].Hi;
                    pass[step.In] = new PRange(step.Val + 1, hi);
                    fail[step.In] = new PRange(lo, step.Val);
                    result.Add((pass, step.Out, 0));
                    result.Add((fail, stepName, stepNum + 1));
                    break;
                case Op.Pipe:
                    result.Add((parts, step.Out, 0));
                    break;
            }

            return result;
        }

        private static ulong PartTwo(Dictionary<string, WorkFlowStep[]> rules)
        {
            var complete = new List<Dictionary<Field, PRange>>();
            var initial = new Dictionary<Field, PRange>()
            {
                { Field.X, new PRange(1, 4000) }, { Field.M, new PRange(1, 4000) },
                { Field.A, new PRange(1, 4000) }, { Field.S, new PRange(1, 4000) }
            };

            var q = new Queue<(Dictionary<Field, PRange>, string, int)>();
            q.Enqueue((initial, "in", 0));

            while (q.Count > 0)
            {
                var (parts, name, stepNum) = q.Dequeue();
                var rule = rules[name];
                var step = rule[stepNum];
                foreach (var res in SplitRange(parts, step, name, stepNum))
                {
                    if (res.Item2 == "A")
                        complete.Add(res.Item1);
                    else if (res.Item2 != "R")
                        q.Enqueue(res);
                }
            }

            ulong total = 0UL;
            foreach (var r in complete)
            {
                ulong a = Convert.ToUInt64(r[Field.X].Hi - r[Field.X].Lo + 1);
                ulong b = Convert.ToUInt64(r[Field.M].Hi - r[Field.M].Lo + 1);
                ulong c = Convert.ToUInt64(r[Field.A].Hi - r[Field.A].Lo + 1);
                ulong d = Convert.ToUInt64(r[Field.S].Hi - r[Field.S].Lo + 1);
                total += a * b * c * d;
            }

            return total;
        }
    }

    enum Op { LT, GT, Pipe }

    internal enum Field { X, M, A, S, Any }

    internal struct WorkFlowStep
    {
        public WorkFlowStep(Field field, string outstr, Op op, int va)
        {
            this.In = field;
            this.Out = outstr;
            this.Op = op;
            this.Val = va;
        }

        public Field In { get; set; }
        public string Out { get; set; }
        public Op Op { get; set; }

        public int Val { get; set; }
    }

    internal struct PRange
    {
        public PRange(int lo, int hi)
        {
            this.Lo = lo;
            this.Hi = hi;
        }

        public int Lo { get; set; }
        public int Hi { get; set; }
    }
}
