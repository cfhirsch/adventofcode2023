using System;
using System.Collections.Generic;
using System.Data;
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

        public string SolvePartTwo(bool test = false)
        {
            var compReg = new Regex(@"(\w+)([<>])(\d+):(\w+)", RegexOptions.Compiled);

            var workflows = new Dictionary<string, string>();

            foreach (string line in PuzzleReader.ReadLines(19, test))
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                int bracketPos = line.IndexOf("{");
                workflows.Add(line.Substring(0, bracketPos),
                              line.Substring(bracketPos + 1, line.Length - bracketPos - 2));
            }

            // Let's try looking at this as a binary tree. The key in each node is an expression.
            // The left node corresponds to the expression being true, the other to its being false.
            // "A" and "R" are leaf nodes. We'll form the union of sets corresponding to "A" leaf nodes.
            string workflowKey = "in";

            var expression = WorkflowToExpression(workflowKey, workflows);

            var baseSet = new Dictionary<string, HashSet<int>>();
            baseSet["x"] = Enumerable.Range(1, 4000).ToHashSet();
            baseSet["m"] = Enumerable.Range(1, 4000).ToHashSet();
            baseSet["a"] = Enumerable.Range(1, 4000).ToHashSet();
            baseSet["s"] = Enumerable.Range(1, 4000).ToHashSet();

            var finalSet = expression.NumThatSatisfy(baseSet);

            long result = 1;
            foreach (HashSet<int> set in finalSet.Values)
            {
                result *= set.Count;
            }

            return result.ToString();
        }

        private Expression WorkflowToExpression(
            string workflowKey,
            Dictionary<string, string> workFlows)
        {
            string[] conditions = workFlows[workflowKey].Split(',');
            Expression prev = null;
            Expression current = null;
            foreach (string condition in conditions)
            {
                Match match = compReg.Match(condition);
                if (match.Success)
                {
                    string category = match.Groups[1].Value;
                    string comp = match.Groups[2].Value;
                    int val = Int32.Parse(match.Groups[3].Value);
                    string label = match.Groups[4].Value;

                    current = ComparisonRuleToExpression(category, comp, val, label, workFlows);
                }
                else
                {
                    switch (condition)
                    {
                        case "R":
                            current = new FalseExpression();
                            break;

                        case "A":
                            current = new TrueExpression();
                            break;

                        default:
                            current = WorkflowToExpression(condition, workFlows);
                            break;
                    }
                }

                if (prev != null)
                {
                    var temp = new CompoundExpression
                    {
                        Left = prev,
                        LogicalOperator = LogicalOperator.Or,
                        Right = current
                    };

                    current = temp;
                }

                prev = current;
            }

            return current;
        }

        private Expression ComparisonRuleToExpression(
            string category,
            string comp,
            int value,
            string label,
            Dictionary<string, string> workFlows)
        {
            var compoundExpression = new CompoundExpression();
            var expression = new ComparisonExpression { Left = category, Value = value };
            compoundExpression.Left = expression;
            compoundExpression.LogicalOperator = LogicalOperator.And;

            switch (comp)
            {
                case ">":
                    expression.Comparator = Comparator.GreaterThan;
                    break;

                case "<":
                    expression.Comparator = Comparator.LessThan;
                    break;

                default:
                    throw new ArgumentException($"Unexpected comparator {comp}.");
            }

            switch (label)
            {
                case "R":
                    compoundExpression.Right = new FalseExpression();
                    break;

                case "A":
                    compoundExpression.Right = new TrueExpression();
                    break;

                default:
                    compoundExpression.Right = WorkflowToExpression(label, workFlows);
                    break;
            }

            return compoundExpression;
        }
    }

    internal abstract class Expression
    {
        public abstract Dictionary<string, HashSet<int>> NumThatSatisfy(Dictionary<string, HashSet<int>> sets);
    }

    internal class ComparisonExpression : Expression
    {
        public string Left { get; set; }

        public Comparator Comparator { get; set; }

        public long Value { get; set; }

        public override Dictionary<string, HashSet<int>> NumThatSatisfy(Dictionary<string, HashSet<int>> sets)
        {
            var newSets = new Dictionary<string, HashSet<int>>();

            foreach (KeyValuePair<string, HashSet<int>> kvp in sets)
            {
                newSets.Add(kvp.Key, kvp.Value.ToHashSet());
            }

            switch (this.Comparator)
            {
                case Comparator.LessThan:
                    newSets[this.Left] = newSets[this.Left].Where(x => x < this.Value).ToHashSet();
                    break;

                case Comparator.LessThanOrEqualTo:
                    newSets[this.Left] = newSets[this.Left].Where(x => x >= this.Value).ToHashSet();
                    break;

                case Comparator.GreaterThan:
                    newSets[this.Left] = newSets[this.Left].Where(x => x > this.Value).ToHashSet();
                    break;

                case Comparator.GreaterThanOrEqualTo:
                    newSets[this.Left] = newSets[this.Left].Where(x => x >= this.Value).ToHashSet();
                    break;

                default:
                    throw new ArgumentException($"Unexpected compartor {this.Comparator}.");
            }

            return newSets;
        }
    }

    internal class CompoundExpression : Expression
    {
        public Expression Left { get; set; }

        public LogicalOperator LogicalOperator { get; set; }

        public Expression Right { get; set; }

        public override Dictionary<string, HashSet<int>> NumThatSatisfy(Dictionary<string, HashSet<int>> sets)
        {
            Dictionary<string, HashSet<int>> left = this.Left.NumThatSatisfy(sets);
            Dictionary<string, HashSet<int>> right = this.Right.NumThatSatisfy(sets);

            var newResult = new Dictionary<string, HashSet<int>>();
            foreach (string key in sets.Keys)
            {
                switch (this.LogicalOperator)
                {
                    case LogicalOperator.And:
                        newResult[key] = left[key].Intersect(right[key]).ToHashSet();
                        break;

                    case LogicalOperator.Or:
                        newResult[key] = left[key].Union(right[key]).ToHashSet();
                        break;

                    default:
                        throw new ArgumentException($"Unexpected operator {this.LogicalOperator}");
                }
            }

            return newResult;
        }
    }

    internal class FalseExpression : Expression
    {
        public override Dictionary<string, HashSet<int>> NumThatSatisfy(Dictionary<string, HashSet<int>> sets)
        {
            return sets.Keys.ToDictionary(x => x, x => new HashSet<int>());
        }
    }

    internal class TrueExpression : Expression
    {
        public override Dictionary<string, HashSet<int>> NumThatSatisfy(Dictionary<string, HashSet<int>> sets)
        {
            return sets.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToHashSet());
        }
    }

    internal enum LogicalOperator
    {
        None,

        And,

        Or
    }

    internal enum Comparator
    {
        None,

        LessThan,

        GreaterThan,

        LessThanOrEqualTo,

        GreaterThanOrEqualTo
    }
}
