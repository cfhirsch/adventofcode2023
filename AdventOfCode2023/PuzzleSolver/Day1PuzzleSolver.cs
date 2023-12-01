using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Utilities;
using Microsoft.SqlServer.Server;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day1PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            int sum = 0;
            foreach (string line in PuzzleReader.ReadLines(1, test))
            {
                List<int> digits = line.Where(x => char.IsDigit(x)).Select(x => Int32.Parse(x.ToString())).ToList();
                sum += 10 * digits.First() + digits.Last();
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            int sum = 0;
            foreach (string line in PuzzleReader.ReadLines(1, test))
            {
                List<int> digits = GetDigits(line).ToList();
                sum += 10 * digits.First() + digits.Last();
            }

            return sum.ToString();
        }

        private static IEnumerable<int> GetDigits(string line)
        {
            int pos = 0;
            while (pos < line.Length)
            {
                if (char.IsDigit(line[pos]))
                {
                    yield return Int32.Parse(line[pos].ToString());
                }
                else
                {
                    string remaining = line.Substring(pos);
                    if (remaining.StartsWith("one"))
                    {
                        yield return 1;
                        
                    }
                    else if (remaining.StartsWith("two"))
                    {
                        yield return 2;
                    }
                    else if (remaining.StartsWith("three"))
                    {
                        yield return 3;
                    }
                    else if (remaining.StartsWith("four"))
                    {
                        yield return 4;
                    }
                    else if (remaining.StartsWith("five"))
                    {
                        yield return 5;
                    }
                    else if (remaining.StartsWith("six"))
                    {
                        yield return 6;
                    }
                    else if (remaining.StartsWith("seven"))
                    {
                        yield return 7;
                    }
                    else if (remaining.StartsWith("eight"))
                    {
                        yield return 8;
                    }
                    else if (remaining.StartsWith("nine"))
                    {
                        yield return 9;
                    }
                }

                pos++;
            }
        }
    }
}
