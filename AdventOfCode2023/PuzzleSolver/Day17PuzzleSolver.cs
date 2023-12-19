using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day17PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            char[,] map = PuzzleReader.ReadMap(17, test);
            int shortest = ShortestPath(map);
            return shortest.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static int ShortestPath(char[,] map)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var startingPoint = new Point(0, 0);
            var stack = new Stack<CityBlock>();

            var sub1 = new CityBlock { Location = startingPoint, Direction = CityDirection.Right };
            var sub2 = new CityBlock { Location = startingPoint, Direction = CityDirection.Down };

            stack.Push(sub1);
            stack.Push(sub2);

            var memoized = new Dictionary<string, int>();

            while (stack.Count > 0)
            {
                Console.SetCursorPosition(10, 0);
                Console.WriteLine($"Stack size: {stack.Count}");
                Console.WriteLine($"Subproblems solved: {memoized.Count}");
                CityBlock current = stack.Pop();

                string key = current.ToString();

                // Are we at the end?
                if (current.Location == new Point(numRows - 1, numCols - 1))
                {
                    memoized[key] = 0;
                }
                else
                {
                    var subproblems = new List<CityBlock>();

                    // Figure out the subproblems.
                    foreach (CityDirection direction in Enum.GetValues(typeof(CityDirection)))
                    {
                        if (direction == CityDirection.None)
                        {
                            continue;
                        }

                        // We can't go back the way we came.
                        if (direction == Opposite(current.Direction))
                        {
                            continue;
                        }

                        // We can go at most three blocks in one direction.
                        if (direction == current.Direction && current.StepsInDirection == 3)
                        {
                            continue;
                        }

                        Point? pt = GetNeighbor(current.Location, direction, numRows, numCols);
                        if (pt.HasValue)
                        {
                            var neighbor = new CityBlock
                            {
                                Location = pt.Value,
                                Direction = direction,
                                StepsInDirection = direction == current.Direction ? current.StepsInDirection + 1 : 1
                            };

                            string neighborKey = neighbor.ToString();

                            if (memoized.ContainsKey(neighborKey))
                            {
                                neighbor.Distance = memoized[neighborKey];
                            }

                            subproblems.Add(neighbor);
                        }
                    }

                    if (subproblems.All(x => x.Distance.HasValue))
                    {
                        string currentKey = current.ToString();
                        int minDistance = Int32.MaxValue;
                        foreach (CityBlock subproblem in subproblems)
                        {
                            int dist = Int32.Parse(map[subproblem.Location.X, subproblem.Location.Y].ToString()) +
                                subproblem.Distance.Value;

                            if (dist < minDistance)
                            {
                                minDistance = dist;
                            }
                        }

                        memoized[currentKey] = minDistance;
                    }
                    else
                    {
                        stack.Push(current);
                        foreach (CityBlock subproblem in subproblems.Where(p => !p.Distance.HasValue))
                        {
                            stack.Push(subproblem);
                        }
                    }
                }
            }

            string sub1Key = sub1.ToString();
            string sub2Key = sub2.ToString();

            return Math.Min(memoized[sub1Key], memoized[sub2Key]);
        }


        private static CityDirection Opposite(CityDirection dir)
        {
            switch (dir)
            {
                case CityDirection.Up:
                    return CityDirection.Down;

                case CityDirection.Down:
                    return CityDirection.Up;

                case CityDirection.Left:
                    return CityDirection.Right;

                case CityDirection.Right:
                    return CityDirection.Left;

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }

        private static Point? GetNeighbor(Point current, CityDirection dir, int maxX, int maxY)
        {
            switch (dir)
            {
                case CityDirection.Up:
                    if (current.X > 0)
                    {
                        return new Point(current.X - 1, current.Y);
                    }

                    break;

                case CityDirection.Down:
                    if (current.X < maxX - 1)
                    {
                        return new Point(current.X + 1, current.Y);
                    }

                    break;

                case CityDirection.Left:
                    if (current.Y > 0)
                    {
                        return new Point(current.X, current.Y - 1);
                    }

                    break;

                case CityDirection.Right:
                    if (current.Y < maxY - 1)
                    {
                        return new Point(current.X, current.Y + 1);
                    }

                    break;

                default:
                    throw new ArgumentException($"Unexpected dir {dir}.");

            }

            return null;
        }
    }

    internal class CityBlock
    {
        public Point Location { get; set; }

        public int? Distance { get; set; }

        public CityDirection Direction { get; set; }

        public int StepsInDirection { get; set; }

        public override int GetHashCode()
        {
            return this.Location.GetHashCode() ^ this.Distance.GetHashCode() ^ this.StepsInDirection.GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.Location.X},{this.Location.Y}),{this.Direction},{this.StepsInDirection}";
        }
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