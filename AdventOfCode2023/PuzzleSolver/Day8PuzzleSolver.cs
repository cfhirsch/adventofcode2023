using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day8PuzzleSolver : IPuzzleSolver
    {
        private static Regex elementRegex = new Regex(@"\((\w+), (\w+)\)", RegexOptions.Compiled);

        public string SolvePartOne(bool test = false)
        {
            string directions = null;
            var map = new Dictionary<string, Tuple<string, string>>();

            bool first = true;
            foreach (string line in PuzzleReader.ReadLines(8, test))
            {
                if (first)
                {
                    directions = line;
                    first = false;
                }
                else if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] lineParts = line.Split(new[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                    Match match = elementRegex.Match(lineParts[1]);
                    map.Add(lineParts[0], new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value));
                }
            }

            string current = "AAA";
            int numSteps = 0;
            int pos = 0;
            while (current != "ZZZ")
            {
                switch (directions[pos])
                {
                    case 'L':
                        current = map[current].Item1;
                        break;

                    case 'R':
                        current = map[current].Item2;
                        break;
                }

                numSteps++;

                pos = (pos + 1) % directions.Length;
            }

            return numSteps.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            string directions = null;
            var map = new Dictionary<string, Tuple<string, string>>();

            bool first = true;
            foreach (string line in PuzzleReader.ReadLines(8, test))
            {
                if (first)
                {
                    directions = line;
                    first = false;
                }
                else if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                else
                {
                    string[] lineParts = line.Split(new[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                    Match match = elementRegex.Match(lineParts[1]);
                    map.Add(lineParts[0], new Tuple<string, string>(match.Groups[1].Value, match.Groups[2].Value));
                }
            }

            string[] current = map.Where(x => x.Key.EndsWith("A")).Select(x => x.Key).ToArray();
            int numSteps = 0;
            int pos = 0;
            int zCount = 0;
            while (zCount < current.Length)
            {
                switch (directions[pos])
                {
                    case 'L':
                        for (int i = 0; i < current.Length; i++)
                        {
                            current[i] = map[current[i]].Item1;
                        }

                        break;

                    case 'R':
                        for (int i = 0; i < current.Length; i++)
                        {
                            current[i] = map[current[i]].Item2;
                        }

                        break;
                }

                numSteps++;

                pos = (pos + 1) % directions.Length;
                zCount = current.Count(x => x.EndsWith("Z"));
            }

            return numSteps.ToString();
        }
    }
}
