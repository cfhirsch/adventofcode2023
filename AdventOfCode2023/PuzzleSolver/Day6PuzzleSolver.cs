using System;
using System.Linq;
using AdventOfCode2023.Utilities;
using Microsoft.Win32;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day6PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var lines = PuzzleReader.ReadLines(6, test).ToList();
            var times = lines[0].Split(':')[1].Trim().Split(
                new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Int32.Parse(s)).ToList();

            var distances = lines[1].Split(':')[1].Trim().Split(
                new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Int32.Parse(s)).ToList();

            int product = 1;
            for (int i = 0; i < times.Count; i++)
            {
                int numWays = 0;
                for (int hold = 0; hold <= times[i]; hold++)
                {
                    // velocity = hold
                    // distance = (time - hold) * velocity
                    int distance = (times[i] - hold) * hold;
                    if (distance > distances[i])
                    {
                        numWays++;
                    }
                }

                product *= numWays;
            }

            return product.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }
    }
}
