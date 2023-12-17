using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day17PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            char[,] map = PuzzleReader.ReadMap(17, test);
            int shortest = FindShortestPath(map);
            return shortest.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static int FindShortestPath(char[,] map)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var dist = new Dictionary<Point, int>();
            var prev = new Dictionary<Point, Point>();
            var queue = new PriorityQueue<CityBlock>();
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (!(i == 0 && j == 0))
                    {
                        var pt = new Point(i, j);
                        dist[pt] = Int32.MaxValue;
                        queue.Enqueue(new CityBlock { Location = pt, Distance = Int32.MaxValue });
                    }
                }
            }

            var source = new Point(0, 0);
            dist[source] = 0;
            queue.Enqueue(new CityBlock { Location = source, Distance = 0 });

            while (queue.Count() > 0)
            {
                CityBlock current = queue.Dequeue();
                foreach (Tuple<Point, CityDirection> neighbor in GetNeighbors(current.Location, numRows, numCols))
                {
                    // We can't reverse direction.
                    if (prev.ContainsKey(current.Location) && prev[current.Location] == neighbor.Item1)
                    {
                        continue;
                    }

                    // We can't go more than 3 steps in one direction.
                    if (neighbor.Item2 == current.Direction && current.StepsInDirection >= 3)
                    {
                        continue;
                    }

                    CityBlock next = queue.Data.FirstOrDefault(b => b.Location == neighbor.Item1);

                    if (next == null)
                    {
                        continue;
                    }

                    next.Direction = neighbor.Item2;
                    if (neighbor.Item2 != current.Direction)
                    {
                        next.StepsInDirection = 1;
                    }
                    else
                    {
                        next.StepsInDirection = current.StepsInDirection + 1;
                    }

                    int alt = current.Distance + Int32.Parse(map[next.Location.X, next.Location.Y].ToString());
                    if (alt < next.Distance)
                    {
                        next.Distance = alt;
                        dist[next.Location] = alt;
                        queue.Reprioritize(next);
                        prev[next.Location] = current.Location;
                    }
                }
            }

            var now = new Point(numRows - 1, numCols - 1);
            while (now != source)
            {
                Console.WriteLine(now);
                now = prev[now];
            }

            return dist[new Point(numRows - 1, numCols - 1)];
        }

        private static IEnumerable<Tuple<Point, CityDirection>> GetNeighbors(Point current, int maxX, int maxY)
        {
            if (current.X > 0)
            {
                yield return new Tuple<Point, CityDirection>(new Point(current.X - 1, current.Y), CityDirection.Up);
            }

            if (current.X < maxX  - 1)
            {
                yield return new Tuple<Point, CityDirection>(new Point(current.X + 1, current.Y), CityDirection.Down);
            }

            if (current.Y > 0)
            {
                yield return new Tuple<Point, CityDirection>(new Point(current.X, current.Y - 1), CityDirection.Left);
            }

            if (current.Y < maxY - 1)
            {
                yield return new Tuple<Point, CityDirection>(new Point(current.X, current.Y + 1), CityDirection.Right);
            }
        }
    }

    internal class CityBlock : IComparable<CityBlock>
    {
        public Point Location { get; set; }

        public int Distance { get; set; }

        public int CompareTo(CityBlock other)
        {
            return this.Distance.CompareTo(other.Distance);
        }

        public CityDirection Direction { get; set; }

        public int StepsInDirection { get; set; }
    }

    internal enum CityDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
}