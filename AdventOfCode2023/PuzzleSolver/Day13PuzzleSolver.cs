using System;
using System.Collections.Generic;
using System.Linq;
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
                List<int> verticalReflections = FindVerticalReflections(map);

                if (verticalReflections.Any())
                {
                    sum += verticalReflections.First();
                }
                else
                {
                    List<int> horizontalReflection = FindHorizontalReflections(map);
                    sum += 100 * horizontalReflection.First();
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            long sum = 0;
            foreach (char[,] map in ReadMaps(test))
            {
                var result = new Tuple<int, Orientation>(-1, Orientation.None);
                List<int> verticalReflections = FindVerticalReflections(map);
                if (verticalReflections.Any())
                {
                    result = new Tuple<int, Orientation>(verticalReflections.First(), Orientation.Vertical);
                }
                else
                {
                    List<int> horizontalReflections = FindHorizontalReflections(map);
                    result = new Tuple<int, Orientation>(horizontalReflections.First(), Orientation.Horizontal);
                }

                bool foundSmudge = false;
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        char[,] newMap = FixSmudge(map, i, j);
                        verticalReflections = FindVerticalReflections(newMap);
                        foreach (int reflection in verticalReflections)
                        {
                            var newResult = new Tuple<int, Orientation>(reflection, Orientation.Vertical);
                            
                            if (!newResult.Equals(result))
                            {
                                sum += newResult.Item1;
                                foundSmudge = true;
                                break;
                            }
                        }

                        if (foundSmudge)
                        {
                            break;
                        }
                        
                        foreach (int reflection in FindHorizontalReflections(newMap))
                        { 
                            var newResult = new Tuple<int, Orientation>(reflection, Orientation.Horizontal);
                            
                            if (!newResult.Equals(result))
                            {
                                sum += 100 * newResult.Item1;
                                foundSmudge = true;
                                break;
                            }
                        }

                        if (foundSmudge)
                        {
                            break;
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

        private List<int> FindHorizontalReflections(char[,] map)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var reflections = new List<int>();

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
                    reflections.Add(i);
                }
            }

            return reflections;
        }

        private List<int> FindVerticalReflections(char[,] map)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var reflections = new List<int>();

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
                    reflections.Add(j);
                }
            }

            return reflections;
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
