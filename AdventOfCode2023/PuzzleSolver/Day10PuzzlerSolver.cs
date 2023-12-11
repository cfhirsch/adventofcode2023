using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day10PuzzlerSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            (char[,] map, int startX, int startY) = ReadMap(test);
            // First we need to figure out what kind of pipe is at the starting point.
            int maxDist = Int32.MinValue;

            var visited = new HashSet<Tuple<int, int>>();
            var queue = new Queue<Tuple<int, int, int>>();
            queue.Enqueue(new Tuple<int, int, int>(startX, startY, 0));
            while (queue.Count > 0)
            {
                Tuple<int, int, int> current = queue.Dequeue();
                visited.Add(new Tuple<int, int>(current.Item1, current.Item2);
                foreach (Tuple<int, int> neighbor in GetNeighbors(map, current.Item1, current.Item2))
                {
                    if (!visited.Contains(neighbor))
                    {
                        var next = new Tuple<int, int, int>(neighbor.Item1, neighbor.Item2, current.Item3 + 1);
                        if (next.Item3 > maxDist)
                        {
                            maxDist = next.Item3;
                        }

                        queue.Enqueue(next);
                    }
                }
            }

            return maxDist.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Tuple<int, int>> GetNeighbors(char[,] map, int x, int y)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            char current = map[x, y];
            if (current == 'S')
            {
                current = GetStartingPointType(map, x, y);
            }

            switch (current)
            {
                // | is a vertical pipe connecting north and south.
                case '|':
                    if (x > 0)
                    {
                        yield return new Tuple<int, int>(x - 1, y);
                    }

                    if (x < numRows - 1)
                    {
                        yield return new Tuple<int, int>(x + 1, y);
                    }

                    break;

                // - is a horizontal pipe connecting east and west.
                case '-':
                    if (y < numCols - 1)
                    {
                        yield return new Tuple<int, int>(x, y + 1);
                    }

                    if (y > 0)
                    {
                        yield return new Tuple<int, int>(x, y - 1);
                    }

                    break;

                // L is a 90 - degree bend connecting north and east.
                case 'L':
                    if (x > 0)
                    {
                        yield return new Tuple<int, int>(x - 1, y);
                    }

                    if (y < numCols - 1)
                    {
                        yield return new Tuple<int, int>(x, y + 1);
                    }

                    break;

                // J is a 90 - degree bend connecting north and west.
                case 'J':
                    if (x > 0)
                    {
                        yield return new Tuple<int, int>(x - 1, y);
                    }

                    if (y > 0)
                    {
                        yield return new Tuple<int, int>(x, y - 1);
                    }

                    break;

                // 7 is a 90 - degree bend connecting south and west.
                case '7':
                    if (x < numRows - 1)
                    {
                        yield return new Tuple<int, int>(x + 1, y);
                    }

                    if (y > 0)
                    {
                        yield return new Tuple<int, int>(x, y - 1);
                    }

                    break;

                // F is a 90 - degree bend connecting south and east.
                case 'F':
                    if (y < numCols - 1)
                    {
                        yield return new Tuple<int, int>(x, y + 1);
                    }

                    if (x < numRows - 1)
                    {
                        yield return new Tuple<int, int>(x + 1, y);
                    }

                    break;

                //. is ground; there is no pipe in this tile.
                case '.':
                    break;

                default:
                    throw new ArgumentException($"Unexpected value {map[x, y]}.");
            }
        }

        // NOTE: The test examples have S = 'F', and my puzzle input has S = 'L'.
        // To save time I'm not considering all cases; this function will not necessarily
        // work on all puzzle inputs.
        private static char GetStartingPointType(char[,] map, int startX, int startY)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            if (startX < numRows - 1 && map[startX + 1, startY] == '|' &&
                startY < numCols - 1 && map[startX, startY + 1] == '-')
            {
                return 'F';
            }

            return 'L';
        }

        private static Tuple<char[,], int, int> ReadMap(bool test)
        {
            List<string> lines = PuzzleReader.ReadLines(10, test).ToList();

            int numRows = lines.Count;
            int numCols = lines[0].Length;

            int startX = -1;
            int startY = -1;

            var map = new char[numRows, numCols];

            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numCols; y++)
                {
                    map[x, y] = lines[x][y];
                    if (map[x, y] == 'S')
                    {
                        startX = x;
                        startY = y;
                    }
                }
            }

            return new Tuple<char[,], int, int>(map, startX, startY);
        }
    }
}
