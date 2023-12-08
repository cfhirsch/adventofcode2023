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
            var periods = new int[current.Length];
            int numSteps = 0;
            int pos = 0;
            int zCount = 0;
            while (zCount < periods.Length)
            {
                numSteps++;
                for (int i = 0; i < current.Length; i++)
                {
                    switch (directions[pos])
                    {
                        case 'L':
                            current[i] = map[current[i]].Item1;
                            break;

                        case 'R':
                            current[i] = map[current[i]].Item2;
                            break;
                    }

                    if (current[i].EndsWith("Z") && periods[i] == 0)
                    {
                        periods[i] = numSteps;
                    }
                }

                pos = (pos + 1) % directions.Length;
                zCount = periods.Count(x => x > 0);
            }

            long result = periods.Aggregate(1L, (lcm, x) => Lcm(lcm, x));
            return result.ToString();
        }

        // The following two methods were adopted from
        // https://www.programming-algorithms.net/article/42865/Least-common-multiple
        private static long Lcm(long a, long b)
        {
            if (a == 0 || b == 0)
            { 
                return 0;
            }

            return (a * b) / Gcd(a, b);
        }

        public static long Gcd(long a, long b)
        {
            if (a < 1 || b < 1)
            {
                throw new ArgumentException("a or b is less than 1");
            }

            do
            {
                long remainder = a % b;
                a = b;
                b = remainder;
            } while (b != 0);

            return a;
        }
    }
}
