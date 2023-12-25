using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day23PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            return Solve(test, isPartTwo: false);
        }

        // I ended up running the solution from here:
        // https://github.com/jonathanpaulson/AdventOfCode/blob/master/2023/23.py
        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            var map = PuzzleReader.ReadMap(23, test);

            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var queue = new Queue<(Point, int, bool[])>();

            var start = new Point(0, 1);
            var countMap = new bool[numRows * numCols];
            countMap[1] = true;

            queue.Enqueue((start, 0, countMap));

            int maxPathLength = Int32.MinValue;
            while (queue.Count > 0)
            {
                (Point current, int count, bool[] counts) = queue.Dequeue();

                if (current.X == numRows - 1)
                {
                    if (count > maxPathLength)
                    {
                        maxPathLength = count;

                        Console.WriteLine($"Found path of length {maxPathLength}.");
                        continue;
                    }
                }

                foreach (Point neighbor in GetNeighbors(map, current, numRows, numCols, isPartTwo))
                {
                    int index = neighbor.X * numCols + neighbor.Y;
                    if (!counts[index])
                    {
                        var nextCountMap = counts.ToArray();
                        nextCountMap[index] = true;
                        queue.Enqueue((neighbor, count + 1, nextCountMap));
                    }
                }
            }

            return maxPathLength.ToString();
        }

        private static IEnumerable<Point> GetNeighbors(char[,] map, Point current, int maxX, int maxY, bool isPartTwo)
        {
            var neighbors = new List<Point>();

            switch (map[current.X, current.Y])
            {
                case 'v':
                    if (isPartTwo)
                    {
                        neighbors = GetNeighborHelper(map, current, maxX, maxY).ToList();
                    }
                    else
                    {
                        neighbors.Add(new Point(current.X + 1, current.Y));
                    }

                    break;

                case '>':
                    if (isPartTwo)
                    {
                        neighbors = GetNeighborHelper(map, current, maxX, maxY).ToList();
                    }
                    else
                    {
                        neighbors.Add(new Point(current.X, current.Y + 1));
                    }

                    break;

                case '.':
                    neighbors = GetNeighborHelper(map, current, maxX, maxY).ToList();
                    break;

                default:
                    throw new ArgumentException($"Unexpected char {map[current.X, current.Y]}");
            }

            return neighbors;
        }

        private static IEnumerable<Point> GetNeighborHelper(char[,] map, Point current, int maxX, int maxY)
        {
            if (current.X > 0 && map[current.X - 1, current.Y] != '#')
            {
                yield return new Point(current.X - 1, current.Y);
            }

            if (current.X < maxX - 1 && map[current.X + 1, current.Y] != '#')
            {
                yield return new Point(current.X + 1, current.Y);
            }

            if (current.Y > 0 && map[current.X, current.Y - 1] != '#')
            {
                yield return new Point(current.X, current.Y - 1);
            }

            if (current.Y < maxY - 1 && map[current.X, current.Y + 1] != '#')
            {
                yield return new Point(current.X, current.Y + 1);
            }
        }
    }
}
