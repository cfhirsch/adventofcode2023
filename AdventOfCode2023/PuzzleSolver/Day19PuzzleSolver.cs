using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day19PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var compReg = new Regex(@"(\w+)([<>])(\d+):(\w+)", RegexOptions.Compiled);

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
            throw new NotImplementedException();
        }
    }
}
