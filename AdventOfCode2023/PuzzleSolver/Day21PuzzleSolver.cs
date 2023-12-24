using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using AdventOfCode2023.Utilities;
using Microsoft.Win32;

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
            var map = PuzzleReader.ReadMap(21, test);
            
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

            int size = numRows;
            int edge = size / 2;

            var stepDict = new Dictionary<int, long>();
            stepDict[edge] = stepDict[edge + size] = stepDict[edge + 2 * size] = 0;

            while (queue.Count > 0)
            {
                (Point current, int numSteps) = queue.Dequeue();

                if (stepDict.ContainsKey(numSteps) && stepDict[numSteps] == 0)
                {
                    stepDict[numSteps] = visited.Where(x => x.Item2 == numSteps).Count();
                }

                if (stepDict.All(x => x.Value > 0))
                {
                    break;
                }

                foreach (Point neighbor in GetNeighborsPartTwo(map, current, numRows, numCols))
                {
                    if (!visited.Contains((neighbor, numSteps + 1)))
                    {   
                        visited.Add((neighbor, numSteps + 1));
                        queue.Enqueue((neighbor, numSteps + 1));
                    }
                }
            }

            long result = Quad(stepDict.Select(x => x.Value).OrderBy(x => x).ToArray(), 26501365);

            return result.ToString();
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

        private IEnumerable<Point> GetNeighborsPartTwo(char[,] map, Point current, int maxX, int maxY)
        {
            var directions = new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };

            foreach ((int dx, int dy) in directions)
            {
                int x = current.X + dx;
                int y = current.Y + dy;
                int modX = Modulo(current.X, maxX);
                int modY = Modulo(current.Y, maxY);

                if (map[modX, modY] != '#')
                {
                    yield return new Point(x, y);
                }
            }
        }

        private static int Modulo(int x, int n)
        { 
            while (x < 0)
            {
                x += n;
            }

            return x % n;
        }

        private static long Quad(long[] y, int n)
        {
            // Use the quadratic formula to find the output at the large steps based on the first three data points
            long a = (y[2] - (2 * y[1]) + y[0]) / 2;
            long b = y[1] - y[0] - a;
            long c = y[0];
            return (a * n * n) + (b * n) + c;
        }
    }
}
