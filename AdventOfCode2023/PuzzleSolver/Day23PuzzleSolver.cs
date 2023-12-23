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
            var map = PuzzleReader.ReadMap(23, test);

            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var queue = new Queue<(Point, HashSet<Point>)>();

            var start = new Point(0, 1);
            var startVisited = new HashSet<Point>();
            startVisited.Add(start);

            queue.Enqueue((start, startVisited));

            int maxPathLength = Int32.MinValue;
            while (queue.Count > 0)
            {
                (Point current, HashSet<Point> visited) = queue.Dequeue();

                if (current.X == numRows - 1)
                {
                    int numSteps = visited.Count - 1;
                    if (numSteps > maxPathLength)
                    {
                        maxPathLength = numSteps;
                        continue;
                    }
                }

                foreach (Point neighbor in GetNeighbors(map, current, numRows, numCols))
                {
                    if (!visited.Contains(neighbor))
                    {
                        var nextVisited = visited.ToHashSet();
                        nextVisited.Add(neighbor);
                        queue.Enqueue((neighbor, nextVisited));
                    }
                }
            }

            return maxPathLength.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Point> GetNeighbors(char[,] map, Point current, int maxX, int maxY)
        {
            switch (map[current.X, current.Y])
            {
                case 'v':
                    yield return new Point(current.X + 1, current.Y);
                    break;

                case '>':
                    yield return new Point(current.X, current.Y + 1);
                    break;

                case '.':
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

                    break;

                default:
                    throw new ArgumentException($"Unexpected char {map[current.X, current.Y]}");
            }
        }
    }
}
