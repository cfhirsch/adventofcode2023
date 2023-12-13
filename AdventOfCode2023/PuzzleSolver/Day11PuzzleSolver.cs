using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day11PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            return Solve(test, 2);
        }

        public string SolvePartTwo(bool test = false)
        {
            return Solve(test, 1000000);
        }

        private static string Solve(bool test, int inflationFactor)
        {
            List<string> map = PuzzleReader.ReadLines(11, test).ToList();
            var emptyRows = new HashSet<int>();
            var emptyColumns = new HashSet<int>();
                
            // Find empty rows.
            for (int i = 0; i < map.Count; i++)
            {
                string row = map[i];
                if (row.All(x => x == '.'))
                {
                    emptyRows.Add(i);
                }
            }

            // Find empty columns.
            var expanded = map.Select(x => new StringBuilder()).ToList();
            for (int i = 0; i < map[0].Length; i++)
            {
                if (map.Select(x => x[i]).All(x => x == '.'))
                {
                    emptyColumns.Add(i);
                }
            }

            // Find the galaxies.
            var galaxies = new List<Tuple<int, int>>();
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == '#')
                    {
                        galaxies.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            long sum = 0;
            // Find sum of shortest paths between galaxies.
            for (int i = 0; i < galaxies.Count - 1; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    Tuple<int, int> g1 = galaxies[i];
                    Tuple<int, int> g2 = galaxies[j];

                    int minX = Math.Min(g1.Item1, g2.Item1);
                    int maxX = Math.Max(g1.Item1, g2.Item1);
                    int minY = Math.Min(g1.Item2, g2.Item2);
                    int maxY = Math.Max(g1.Item2, g2.Item2);

                    int dist = 0;
                    for (int x = minX + 1; x <= maxX; x++)
                    {
                        dist += emptyRows.Contains(x) ? inflationFactor : 1;
                    }

                    for (int y = minY + 1; y <= maxY; y++)
                    {
                        dist += emptyColumns.Contains(y) ? inflationFactor : 1;
                    }

                    sum += dist;
                }
            }

            return sum.ToString();
        }
    }
}
