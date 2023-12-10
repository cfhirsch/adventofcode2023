using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day9PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var sequences = new List<List<int>>();
            foreach (string line in PuzzleReader.ReadLines(9, test))
            {
                sequences.Add(line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select
                    (x => Int32.Parse(x)).ToList());
            }

            long sum = 0;
            foreach (List<int> sequence in sequences)
            {
                var differences = new List<List<int>>();
                bool allZeroes = false;
                List<int> current = sequence;
                differences.Add(current);
                while (!allZeroes)
                {
                    var diffs = new List<int>();
                    for (int i = 1; i < current.Count; i++)
                    {
                        diffs.Add(current[i] - current[i - 1]);
                    }

                    differences.Add(diffs);
                    current = diffs;

                    allZeroes = diffs.All(x => x == 0);
                }

                for (int i = differences.Count - 2; i >= 0; i--)
                {
                    differences[i].Add(differences[i].Last() + differences[i + 1].Last());
                }

                sum += sequence.Last();
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }
    }
}
