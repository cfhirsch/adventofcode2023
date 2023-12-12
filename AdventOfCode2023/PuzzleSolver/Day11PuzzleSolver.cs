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
            List<string> map = PuzzleReader.ReadLines(11, test).ToList();

            var temp = new List<string>();

            // Expand rows.
            foreach (string row in map)
            {
                temp.Add(row);
                if (row.All(x => x == '.'))
                {
                    temp.Add(row);
                }
            }

            map = temp;

            // Expand columns.
            var expanded = map.Select(x => new StringBuilder()).ToList();
            for (int i = 0; i < map[0].Length; i++)
            {
                bool empty = map.Select(x => x[i]).All(x => x == '.');
                for (int j = 0; j < expanded.Count; j++)
                {
                    expanded[j].Append(map[j][i]);
                    if (empty)
                    {
                        expanded[j].Append(map[j][i]);
                    }
                }
            }

            map = expanded.Select(sb => sb.ToString()).ToList();

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
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i; j < galaxies.Count; j++)
                {
                    sum += Math.Abs(galaxies[i].Item1 - galaxies[j].Item1) +
                           Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static void PrintMap(List<string> map)
        {
            foreach (string s in map)
            {
                Console.WriteLine(s);
            }
        }
    }
}
