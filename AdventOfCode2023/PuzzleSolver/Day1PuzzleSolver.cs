using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Utilities;

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
            throw new NotImplementedException();
        }
    }
}
