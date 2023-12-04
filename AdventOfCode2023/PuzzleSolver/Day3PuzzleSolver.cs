using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day3PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            char[,] schematic = ParseInput(test);
            int length = schematic.GetLength(0);
            int height = schematic.GetLength(1);

            bool digit = false;
            bool isPartNumber = false;
            StringBuilder sb = new StringBuilder();
            long sum = 0;
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (char.IsDigit(schematic[x, y]))
                    {
                        if (!digit)
                        {
                            digit = true;
                            isPartNumber = false;
                            sb = new StringBuilder();
                        }
                        
                        sb.Append(schematic[x, y]);

                        foreach (Tuple<int, int> neighbor in GetNeighbors(x, y, length, height))
                        {
                            char symbol = schematic[neighbor.Item1, neighbor.Item2];
                            if (!char.IsDigit(symbol) && symbol != '.')
                            {
                                isPartNumber = true;
                            }
                        }
                    }
                    else
                    {
                        if (digit)
                        {
                            digit = false;

                            if (isPartNumber)
                            {
                                sum += Int32.Parse(sb.ToString());
                                isPartNumber = false;
                            }
                        }
                    }
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            char[,] schematic = ParseInput(test);
            int length = schematic.GetLength(0);
            int height = schematic.GetLength(1);

            var digit = false;
            var gears = new Dictionary<Tuple<int, int>, List<int>>();
            var sb = new StringBuilder();
            var adjacentGears = new HashSet<Tuple<int, int>>();
            long sum = 0;
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (char.IsDigit(schematic[x, y]))
                    {
                        if (!digit)
                        {
                            digit = true;
                            sb = new StringBuilder();
                        }

                        sb.Append(schematic[x, y]);

                        foreach (Tuple<int, int> neighbor in GetNeighbors(x, y, length, height))
                        {
                            char symbol = schematic[neighbor.Item1, neighbor.Item2];
                            if (symbol == '*')
                            {
                                if (!gears.ContainsKey(neighbor))
                                {
                                    gears.Add(neighbor, new List<int>());
                                }

                                adjacentGears.Add(neighbor);
                            }
                        }
                    }
                    else
                    {
                        if (digit)
                        {
                            digit = false;
                            int partNumber = Int32.Parse(sb.ToString());
                            foreach (Tuple<int, int> gear in adjacentGears)
                            {
                                gears[gear].Add(partNumber);
                            }

                            adjacentGears.Clear();
                        }
                    }
                }
            }

            foreach (KeyValuePair<Tuple<int, int>, List<int>> kvp in gears)
            {
                if (kvp.Value.Count == 2)
                {
                    sum += kvp.Value[0] * kvp.Value[1];
                }
            }

            return sum.ToString();

        }

        private static char[,] ParseInput(bool test)
        {
            var lines = PuzzleReader.ReadLines(3, test).ToList();

            var result = new char[lines.Count, lines[0].Length];

            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[0].Length; j++)
                {
                    result[i, j] = lines[i][j];
                }
            }

            return result;
        }

        private static IEnumerable<Tuple<int, int>> GetNeighbors(int i, int j, int length, int height)
        {
            for (int x = i - 1; x <= i + 1; x++)
            {
                for (int y = j - 1; y <= j + 1; y++)
                {
                    if (!(x == i && y == j) && x >= 0 && x < length && y >= 0 && y < height)
                    {
                        yield return new Tuple<int, int>(x, y);
                    }
                }
            }
        }
    }
}
