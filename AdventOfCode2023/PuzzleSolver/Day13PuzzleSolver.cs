using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
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
                int? verticalReflection = FindVerticalReflection(map);

                if (verticalReflection.HasValue)
                {
                    sum += verticalReflection.Value;
                }
                else
                {
                    int? horizontalReflection = FindHorizontalReflection(map);
                    sum += 100 * horizontalReflection.Value;
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            long sum = 0;
            foreach (char[,] map in ReadMaps(test))
            {
                ConsoleUtilities.PrintMap(map);

                var result = new Tuple<int, Orientation>(-1, Orientation.None);

                int? verticalReflection = FindVerticalReflection(map);

                if (verticalReflection.HasValue)
                {
                    result = new Tuple<int, Orientation>(verticalReflection.Value, Orientation.Vertical);
                    Console.WriteLine($"Original: vertical reflection line on {result.Item1}.");
                    Console.WriteLine();
                }
                else
                {
                    int? horizontalReflection = FindHorizontalReflection(map);
                    result = new Tuple<int, Orientation>(horizontalReflection.Value, Orientation.Horizontal);
                    Console.WriteLine($"Original: horizontal reflection line on {result.Item1}.");
                    Console.WriteLine();
                }

                bool foundSmudge = false;
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        char[,] newMap = FixSmudge(map, i, j);
                        verticalReflection = FindVerticalReflection(newMap);
                        if (verticalReflection.HasValue)
                        {
                            var newResult = new Tuple<int, Orientation>(verticalReflection.Value, Orientation.Vertical);

                            if (newResult != result)
                            {
                                Console.WriteLine($"Correction: vertical reflection line on {verticalReflection.Value}.");
                                Console.WriteLine();

                                sum += verticalReflection.Value;
                                foundSmudge = true;
                                break;
                            }
                        }
                        
                        int? horReflection = FindHorizontalReflection(newMap);
                        if (horReflection.HasValue)
                        {
                            var newResult = new Tuple<int, Orientation>(horReflection.Value, Orientation.Horizontal);
                            if (newResult != result)
                            {
                                Console.WriteLine($"Correction: horizontal reflection line on {horReflection.Value}.");
                                Console.WriteLine();

                                sum += 100 * horReflection.Value;
                                foundSmudge = true;
                                break;
                            }
                        }
                    }

                    if (foundSmudge)
                    {
                        break;
                    }
                }
            }

            return sum.ToString();
        }

        private int? FindHorizontalReflection(char[,] map)
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
                    return i;
                }
            }

            return null;
        }

        private int? FindVerticalReflection(char[,] map)
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
                    return j;
                }
            }

            return null;
        }

        private char[,] FixSmudge(char[,] map, int x, int y)
        {
            var newMap = new char[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < newMap.GetLength(0); i++)
            { 
                for (int j = 0; j < newMap.GetLength(1); j++)
                {
                    if (i == x && j == y)
                    {
                        newMap[i, j] = (map[i, j] == '.') ? '#' : '.';
                    }
                    else
                    {
                        newMap[i, j] = map[i, j];
                    }
                }
            }

            return newMap;
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

                    char[,] map = lineList.ToCharArray();
                    lineList = new List<string>();
                    yield return map;
                }
                else
                {
                    lineList.Add(line);
                }
            }

            yield return lineList.ToCharArray();
        }
    }

    internal enum Orientation
    {
        None,
        Horizontal,
        Vertical
    }
}
