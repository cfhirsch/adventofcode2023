using System;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day15PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            string[] steps = PuzzleReader.ReadLines(15, test).First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            long sum = steps.Sum(x => Hash(x));
            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static long Hash(string s)
        {
            long hash = 0;
            foreach (char ch in s)
            {
                hash += ch;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }
    }
}
