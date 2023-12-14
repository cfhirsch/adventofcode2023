using System;
using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day14PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var map = ReadMap(test);
            if (test)
            {
                ConsoleUtilities.PrintMap(map);
            }

            map = RollNorth(map);
            if (test)
            {
                ConsoleUtilities.PrintMap(map);
            }

            long totalLoad = NorthLoad(map);
            return totalLoad.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            var visited = new Dictionary<string, int>();
            char[,] map = ReadMap(test);
            
            int cycle = 0;
            int remainder = -1;
            visited.Add(CharArrayToString(map), cycle);
            while (cycle < 100000)
            {
                cycle++;
                map = Cycle(map);
                string key = CharArrayToString(map);
                if (visited.ContainsKey(key))
                {
                    int cycleStart = visited[key];
                    int cycleLength = cycle - cycleStart;

                    // 1000000000 cycles
                    // steps_to_beginning_of_cycle + k * cycle_length + remainder
                    // (1000000000 - steps_to_beginning_of_cycle) / cycle_length + remainder
                    // Run remainder steps to get the answer.

                    remainder = (1000000000 - cycleStart) % cycleLength;
                    break;
                }
                else
                {
                    visited.Add(key, cycle);
                }
            }

            for (int i = 0; i < remainder; i++)
            {
                map = Cycle(map);
            }

            long totalLoad = NorthLoad(map);
            return totalLoad.ToString();
        }

        private char[,] Cycle(char[,] map)
        {
            map = RollNorth(map);
            map = RollWest(map);
            map = RollSouth(map);
            map = RollEast(map);

            return map;
        }

        private static char[,] ReadMap(bool test)
        {
            return PuzzleReader.ReadLines(14, test).ToList().ToCharArray();
        }

        private static long NorthLoad(char[,] map)
        {
            long totalLoad = 0;
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (map[i, j] == 'O')
                    {
                        totalLoad += numRows - i;
                    }
                }
            }

            return totalLoad;
        }

        private static char[,] RollNorth(char[,] map)
        {
            // In the ith iteration of this loop, we move any rock we find in each 
            // column j in the ith row "north" as far as we can.
            for (int i = 1; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'O')
                    {
                        int k = i - 1;
                        while (k >= 0 && map[k, j] == '.')
                        {
                            map[k + 1, j] = '.';
                            map[k, j] = 'O';
                            k--;
                        }
                    }
                }
            }

            return map;
        }

        private static char[,] RollWest(char[,] map)
        {
            // In the jth iteration of this loop, we move any rock we find in each 
            // row i in the jth column "west" as far as we can.
            for (int j = 0; j < map.GetLength(1); j++)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    if (map[i, j] == 'O')
                    {
                        int k = j - 1;
                        while (k >= 0 && map[i, k] == '.')
                        {
                            map[i, k + 1] = '.';
                            map[i, k] = 'O';
                            k--;
                        }
                    }
                }
            }

            return map;
        }

        private static char[,] RollSouth(char[,] map)
        {
            // In the ith iteration of this loop, we move any rock we find in each 
            // column j in the ith row "south" as far as we can.
            for (int i = map.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'O')
                    {
                        int k = i + 1;
                        while (k < map.GetLength(0) && map[k, j] == '.')
                        {
                            map[k - 1, j] = '.';
                            map[k, j] = 'O';
                            k++;
                        }
                    }
                }
            }

            return map;
        }

        private static char[,] RollEast(char[,] map)
        {
            // In the jth iteration of this loop, we move any rock we find in each 
            // row i in the jth column "east" as far as we can.
            for (int j = map.GetLength(1) - 1; j >= 0; j--)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    if (map[i, j] == 'O')
                    {
                        int k = j + 1;
                        while (k < map.GetLength(1) && map[i, k] == '.')
                        {
                            map[i, k - 1] = '.';
                            map[i, k] = 'O';
                            k++;
                        }
                    }
                }
            }

            return map;
        }

        private static string CharArrayToString(char[,] charArray)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < charArray.GetLength(0); i++)
            {
                for (int j = 0; j < charArray.GetLength(1); j++)
                {
                    sb.Append(charArray[i, j]);
                }
            }

            return sb.ToString();
        }
    }
}
