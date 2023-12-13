using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day12PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            long counts = 0;
            foreach (string line in PuzzleReader.ReadLines(12, test))
            {
                string[] lineParts = line.Split(new[] { ' ' });
                string row = lineParts[0];
                List<int> numbers = lineParts[1].Split(new[] { ',' }).Select(x => Int32.Parse(x)).ToList();
                var memoized = new Dictionary<string, long>();
                counts += GetNumCombos(row, numbers, memoized);
            }

            return counts.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static long GetNumCombos(string row, List<int> numbers, Dictionary<string, long> memoized)
        {
            string key = $"{row}:{string.Join(",", numbers.ToArray())}";
            if (memoized.ContainsKey(key))
            {
                return memoized[key];
            }

            long sum = 0;
            if (row.Length == 0)
            {
                sum = numbers.Any() ? 0 : 1;
            }
            else if (!numbers.Any())
            {
                sum = row.Any(s => s == '#') ? 0 : 1;
            }
            else if (row.Count(x => x == '#' || x == '?') < numbers.Sum(x => x))
            {
                sum = 0;
            }
            else
            {
                // First char is '.' or '?'
                if (row[0] != '#')
                {
                    sum += GetNumCombos(row.Substring(1), numbers, memoized);
                }

                // First char is '#' or '?'
                if (row[0] != '.')
                {
                    string prefix = row.Substring(0, numbers[0]);
                    if (prefix.All(x => x == '#' || x == '?'))
                    {
                        // Case 1: there are numbers[0] chars remaining in the string. 
                        if (prefix.Length == row.Length)
                        {
                            sum += numbers.Count > 1 ? 0 : 1;
                        }
                        // Case 2: only 1 characters left after the first numbers[0] chars.
                        else if (row.Length == prefix.Length + 1)
                        {
                            sum += row[numbers[0]] == '#' ? 0 : 
                                GetNumCombos(string.Empty, numbers.Skip(1).ToList(), memoized);
                        }
                        else
                        {
                            sum += row[numbers[0]] == '#' ? 0 :
                                GetNumCombos(row.Substring(numbers[0] + 1), numbers.Skip(1).ToList(), memoized);
                        }
                    }
                }
            }

            memoized[key] = sum;
            return sum;
        }
    }

    internal enum ParseState
    {
        Begin,
        Broken,
        Healthy
    }
}
