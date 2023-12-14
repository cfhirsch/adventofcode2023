using System;
using AdventOfCode2023.Utilities;
using System.Collections.Generic;
using System.Linq;

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

            return totalLoad.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static char[,] ReadMap(bool test)
        {
            return PuzzleReader.ReadLines(14, test).ToList().ToCharArray();
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
    }
}
