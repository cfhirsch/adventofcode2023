using System;
using System.Collections.Generic;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day13PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            long sum = 0;
            foreach (char[,] map in ReadMaps(test))
            {
                PrintMap(map);

                foreach (int verticalReflection in FindVerticalReflection(map))
                {
                    Console.WriteLine($"Vertical reflection at {verticalReflection}.");
                    Console.WriteLine();
                    sum += verticalReflection;
                }
                
                foreach (int horizontalReflection in FindHorizontalReflection(map))
                {
                    Console.WriteLine($"Horizontal reflection at {horizontalReflection}.");
                    Console.WriteLine();
                    sum += 100 * horizontalReflection;
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<int> FindHorizontalReflection(char[,] map)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            // e.g. for 8 columns, 
            // 0|1
            // 0,1|2,3
            // 0,1,2|3,4,5
            // 0,1,2,3|4,5,6,7
            // 2,3,4|5,6,7
            // 4,5|6,7
            // 6/7
            for (int i = 1; i < numRows; i++)
            {
                int reflectionSize = (i <= numRows / 2) ? i : numRows - i;
                int above = i - 1;
                int below = i;
                int step = 1;
                bool reflection = true;
                while (step <= reflectionSize)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        if (map[above, j] != map[below, j])
                        {
                            reflection = false;
                            break;
                        }
                    }

                    if (!reflection)
                    {
                        break;
                    }

                    above--;
                    below++;
                    step++;
                }

                if (reflection)
                {
                    yield return i;
                }
            }
        }

        private IEnumerable<int> FindVerticalReflection(char[,] map)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            // e.g. for 8 columns, 
            // 0|1
            // 0,1|2,3
            // 0,1,2|3,4,5
            // 0,1,2,3|4,5,6,7
            // 2,3,4|5,6,7
            // 4,5|6,7
            // 6/7
            for (int j = 1; j < numCols; j++)
            {
                int reflectionSize = (j <= numCols / 2) ? j : numCols - j;
                int left = j - 1;
                int right = j;
                int step = 1;
                bool reflection = true;
                while (step <= reflectionSize)
                {
                    for (int i = 0; i < numRows; i++)
                    {
                        if (map[i, left] != map[i, right])
                        {
                            reflection = false;
                            break;
                        }
                    }

                    if (!reflection)
                    {
                        break;
                    }

                    left--;
                    right++;
                    step++;
                }

                if (reflection)
                {
                    yield return j;
                }
            }
        }

        private static void PrintMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private static IEnumerable<char[,]> ReadMaps(bool test)
        {
            var lines = PuzzleReader.ReadLines(13, test);
            var lineList = new List<string>();
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    int numRows = lineList.Count;
                    int numCols = lineList[0].Length;

                    char[,] map = ListToCharArray(lineList);
                    lineList = new List<string>();
                    yield return map;
                }
                else
                {
                    lineList.Add(line);
                }
            }

            yield return ListToCharArray(lineList);
        }

        private static char[,] ListToCharArray(List<string> lineList)
        {
            int numRows = lineList.Count;
            int numCols = lineList[0].Length;

            char[,] map = new char[numRows, numCols];

            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numCols; y++)
                {
                    map[x, y] = lineList[x][y];
                }
            }

            return map;
        }
    }
}
