using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day21PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var map = PuzzleReader.ReadMap(21, test);
            int maxSteps = test ? 6 : 64;

            Point start = new Point(-1, -1);
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            bool foundStart = false;
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (map[i, j] == 'S')
                    {
                        start = new Point(i, j);
                        foundStart = true;
                        break;
                    }
                }

                if (foundStart)
                {
                    break;
                }
            }

            var queue = new Queue<(Point, int)>();
            queue.Enqueue((start, 0));

            var visited = new HashSet<(Point, int)>();
            visited.Add((start, 0));

            while (queue.Count > 0)
            {
                (Point current, int numSteps) = queue.Dequeue();
                if (numSteps < maxSteps)
                {
                    foreach (Point neighbor in GetNeighbors(map, current, numRows, numCols))
                    {
                        if (!visited.Contains((neighbor, numSteps + 1)))
                        {
                            visited.Add((neighbor, numSteps + 1));
                            queue.Enqueue((neighbor, numSteps + 1));
                        }
                    }
                }
            }

            return visited.Where(x => x.Item2 == maxSteps).Count().ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Point> GetNeighbors(char[,] map, Point current, int maxX, int maxY)
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
