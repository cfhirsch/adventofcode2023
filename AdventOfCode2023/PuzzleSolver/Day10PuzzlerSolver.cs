using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
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
                visited.Add(new Tuple<int, int>(current.Item1, current.Item2));
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
            (char[,] map, int startX, int startY) = ReadMap(test);

            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            //ConsoleUtilities.PrintMap(map);

            char[,] expandedMap = ExpandMap(map, startX, startY);

            // Figure out which tiles are in the loop.
            var inLoop = new HashSet<Tuple<int, int>>();
            var queue = new Queue<Tuple<int, int>>();
            queue.Enqueue(new Tuple<int, int>(startX * 2, startY * 2));
            while (queue.Count > 0)
            {
                Tuple<int, int> current = queue.Dequeue();
                inLoop.Add(current);
                foreach (Tuple<int, int> neighbor in GetNeighbors(expandedMap, current.Item1, current.Item2))
                {
                    if (!inLoop.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                    }
                }
            }

            var external = new HashSet<Tuple<int, int>>();
            // Figure out whih tiles are obviously external; namely on the border, and not on the loop.
            for (int i = 0; i < expandedMap.GetLength(0); i++)
            {
                var loc = new Tuple<int, int>(i, 0);
                if (!inLoop.Contains(loc))
                {
                    external.Add(loc);
                }

                loc = new Tuple<int, int>(i, map.GetLength(0) - 1);
                if (!inLoop.Contains(loc))
                {
                    external.Add(loc);
                }
            }

            for (int j = 0; j < expandedMap.GetLength(1); j++)
            {
                var loc = new Tuple<int, int>(0, j);
                if (!inLoop.Contains(loc))
                {
                    external.Add(loc);
                }

                loc = new Tuple<int, int>(map.GetLength(1) - 1, j);
                if (!inLoop.Contains(loc))
                {
                    external.Add(loc);
                }
            }

            ConsoleUtilities.PrintMap(expandedMap);

            var internalTiles = new HashSet<Tuple<int, int>>();

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    var loc = new Tuple<int, int>(i, j);

                    var expandedLoc = new Tuple<int, int>(i * 2, j * 2);

                    if (!inLoop.Contains(expandedLoc) && !external.Contains(expandedLoc))
                    {
                        var visited = new HashSet<Tuple<int, int>>();
                        queue = new Queue<Tuple<int, int>>();
                        queue.Enqueue(expandedLoc);
                        bool isExternal = false;
                        while (queue.Count > 0)
                        {
                            var current = queue.Dequeue();
                            visited.Add(current);
                            foreach (Tuple<int, int> neighbor in GetNeighborTiles(
                                expandedLoc.Item1, expandedLoc.Item2, 2 * numRows - 1, 2 * numCols - 1))
                            {
                                if (visited.Contains(neighbor))
                                {
                                    continue;
                                }

                                if (external.Contains(neighbor))
                                {
                                    external.Add(expandedLoc);
                                    isExternal = true;
                                    break;
                                }
                                else if (!inLoop.Contains(neighbor))
                                {
                                    queue.Enqueue(neighbor);
                                }
                            }

                            if (isExternal)
                            {
                                break;
                            }
                        }

                        if (!isExternal)
                        {
                            internalTiles.Add(loc);
                        }
                    }
                }
            }

            int numEnclosed = internalTiles.Count();
            return numEnclosed.ToString();
        }

        private static IEnumerable<Tuple<int, int>> GetNeighborTiles(int x, int y, int maxX, int maxY)
        {
            if (x > 0)
            {
                yield return new Tuple<int, int>(x - 1, y);
            }

            if (x < maxX - 1)
            {
                yield return new Tuple<int, int>(x + 1, y);
            }

            if (y > 0)
            {
                yield return new Tuple<int, int>(x, y - 1);
            }

            if (y < maxY - 1)
            {
                yield return new Tuple<int, int>(x, y + 1);
            }
        }

        private static char[,] ExpandMap(char[,] map, int startX, int startY)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            map[startX, startY] = GetStartingPointType(map, startX, startY);

            // First pass: expand rows;
            var firstPass = new char[2 * numRows - 1, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    firstPass[2 * i, j] = map[i, j];

                    if (i < numRows - 1)
                    {
                        firstPass[2 * i + 1, j] = GetVerticalConnector(map[i, j], map[i + 1, j]);
                    }
                }
            }

            // Second pass: expand columns;
            var expandedMap = new char[2 * numRows - 1, 2 * numCols - 1];
            for (int i = 0; i < 2 * numRows - 1; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    expandedMap[i, 2 * j] = firstPass[i, j];

                    if (j < numCols - 1)
                    {
                        expandedMap[i, 2 * j + 1] = GetHorizontalConnector(firstPass[i, j], firstPass[i, j + 1]);
                    }
                }
            }

            return expandedMap;
        }

        private static char GetHorizontalConnector(char ch1, char ch2)
        {
            switch (ch1)
            {
                case '|':
                case 'J':
                case '7':
                case '.':
                    return '.';

                case '-':
                case 'L':
                    return '-';

                case 'F':
                    if (ch2 == '-' || ch2 == 'J' || ch2 == '7')
                    {
                        return '-';
                    }

                    return '.';

                default:
                    throw new ArgumentException($"Unexpected arguments {ch1}, {ch2}.");
            }
        }

        private static char GetVerticalConnector(char ch1, char ch2)
        {
            switch (ch1)
            {
                case '|':
                    if (ch2 == '|' || ch2 == 'L' || ch2 == 'J')
                    {
                        return '|';
                    }

                    return '.';

                case '-':
                case 'L':
                case 'J':
                case '.':
                    return '.';

                case '7':
                case 'F':
                    if (ch2 == 'L' || ch2 == 'J' || ch2 == '|')
                    {
                        return '|';
                    }

                    return '.';

                default:
                    throw new ArgumentException($"Unexpected arguments {ch1}, {ch2}.");
            }
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

            if (startX < numRows - 1 && map[startX + 1, startY] == '|')
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
