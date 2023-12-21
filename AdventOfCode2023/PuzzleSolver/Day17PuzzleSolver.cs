using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day17PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            char[,] map = PuzzleReader.ReadMap(17, test);
            int shortest = ShortestPath(map, test);
            return shortest.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static int ShortestPath(char[,] map, bool test)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);
            
            var startingPoint = new Point(0, 0);

            var sb1 = new CityBlock
            {
                Location = startingPoint,
                Direction = CityDirection.Right
            };

            var sb2 = new CityBlock
            {
                Location = startingPoint,
                Direction = CityDirection.Down
            };

            var cameFrom = new Dictionary<CityBlock, CityBlock>();
            var fScores = new Dictionary<string, int>();
            var gScores = new Dictionary<string, int>();

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    foreach (CityDirection dir in Enum.GetValues(typeof(CityDirection)))
                    {
                        if (dir == CityDirection.None)
                        {
                            continue;
                        }

                        for (int k = 0; k <= 3; k++)
                        {
                            if (k == 0 && (i > 0 || j > 0))
                            {
                                continue;
                            }

                            string key = $"({i},{j}),{dir},{k}";

                            fScores[key] = gScores[key] = Int32.MaxValue;
                        }
                    }
                }
            }

            sb1.GScore = sb2.GScore = 0;
            sb1.FScore = sb2.FScore = numRows - 1 + numCols - 1;

            var openSet = new PriorityQueue<CityBlock>();

            gScores[sb1.ToString()] = gScores[sb2.ToString()] = 0;
            fScores[sb1.ToString()] = fScores[sb1.ToString()] = numRows - 1 + numCols - 1;

            openSet.Enqueue(sb1);
            openSet.Enqueue(sb2);

            var target = new Point(numRows - 1, numCols - 1);

            int minCost = Int32.MaxValue;

            int maxX = 0, maxY = 0;
            while (openSet.Count() > 0)
            {
                CityBlock current = openSet.Dequeue();

                if (current.Location == target)
                {
                    if (test)
                    {
                        ReconstructPath(map, cameFrom, current);
                    }

                    minCost = current.GScore;

                    break;
                }

                foreach (CityBlock neighbor in GetNeighbors(current, numRows, numCols))
                {
                    int tentativeGScore = gScores[current.ToString()] + Int32.Parse(map[neighbor.Location.X, neighbor.Location.Y].ToString());
                    if (tentativeGScore < gScores[neighbor.ToString()])
                    {
                        cameFrom[neighbor] = current;
                        gScores[neighbor.ToString()] = neighbor.GScore = tentativeGScore;
                        fScores[neighbor.ToString()] = neighbor.FScore = tentativeGScore + (numRows - neighbor.Location.X - 1) + (numCols - neighbor.Location.Y - 1);
                        openSet.Enqueue(neighbor);
                    }
                }
            }
                
            return minCost;
        }

        private static void ReconstructPath(
            char[,] map,
            Dictionary<CityBlock, CityBlock> cameFrom,
            CityBlock current)
        {
            map[current.Location.X, current.Location.Y] = CityDirectionToString(current.Direction); 

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                map[current.Location.X, current.Location.Y] = CityDirectionToString(current.Direction);
            }

            ConsoleUtilities.PrintMap(map);
        }

        private static char CityDirectionToString(CityDirection dir)
        {
            switch (dir)
            {
                case CityDirection.None:
                    return '.';

                case CityDirection.Left:
                    return '<';

                case CityDirection.Right:
                    return '>';

                case CityDirection.Up:
                    return '^';

                case CityDirection.Down:
                    return 'v';

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
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

        private static IEnumerable<CityBlock> GetNeighbors(CityBlock block, int maxX, int maxY)
        {
            Point point = block.Location;

            Point nextPoint;
            CityDirection cityDirection;
            CityBlock neighbor;

            // Turn left.
            (cityDirection, nextPoint) = TurnLeft(block.Direction, point);
            if (InBounds(nextPoint, maxX, maxY))
            {
                neighbor = new CityBlock { Location = nextPoint, Direction = cityDirection, StepsInDirection = 1 };
                yield return neighbor;
            }

            // Turn right.
            (cityDirection, nextPoint) = TurnRight(block.Direction, point);
            if (InBounds(nextPoint, maxX, maxY))
            {
                neighbor = new CityBlock { Location = nextPoint, Direction = cityDirection, StepsInDirection = 1 };
                yield return neighbor;
            }

            // Go forward if we can.
            if (block.StepsInDirection < 3)
            {
                int stepsInDirection = block.StepsInDirection + 1;
                nextPoint = GoForward(block.Direction, point);
                if (InBounds(nextPoint, maxX, maxY))
                {
                   neighbor = new CityBlock { Location = nextPoint, Direction = block.Direction, StepsInDirection = stepsInDirection };
                    yield return neighbor;
                }
            }
        }

        private static bool InBounds(Point pt, int maxX, int maxY)
        {
            return pt.X >= 0 && pt.X < maxX && pt.Y >= 0 && pt.Y < maxY;
        }

        private static Point GoForward(CityDirection dir, Point pt)
        {
            switch (dir)
            {
                case CityDirection.Up:
                    return new Point(pt.X - 1, pt.Y);

                case CityDirection.Down:
                    return new Point(pt.X + 1, pt.Y);

                case CityDirection.Left:
                    return new Point(pt.X, pt.Y - 1);

                case CityDirection.Right:
                    return new Point(pt.X, pt.Y + 1);

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }

        private static (CityDirection, Point) TurnLeft(CityDirection dir, Point pt)
        {
            switch (dir)
            {
                case CityDirection.Up:
                    return (CityDirection.Left, new Point(pt.X, pt.Y - 1));

                case CityDirection.Down:
                    return (CityDirection.Right, new Point(pt.X, pt.Y + 1));

                case CityDirection.Left:
                    return (CityDirection.Down, new Point(pt.X + 1, pt.Y));

                case CityDirection.Right:
                    return (CityDirection.Up, new Point(pt.X - 1, pt.Y));

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }

        private static (CityDirection, Point) TurnRight(CityDirection dir, Point pt)
        {
            switch (dir)
            {
                case CityDirection.Up:
                    return (CityDirection.Right, new Point(pt.X, pt.Y + 1));

                case CityDirection.Down:
                    return (CityDirection.Left, new Point(pt.X, pt.Y - 1));

                case CityDirection.Left:
                    return (CityDirection.Up, new Point(pt.X - 1, pt.Y));

                case CityDirection.Right:
                    return (CityDirection.Down, new Point(pt.X + 1, pt.Y));

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");
            }
        }
    }

    internal class CityBlock : IComparable<CityBlock>
    {
        public CityBlock()
        {
            this.FScore = Int32.MaxValue;
            this.GScore = Int32.MaxValue;
        }

        public Point Location { get; set; }

        public int FScore { get; set; }

        public int GScore { get; set; }

        public CityDirection Direction { get; set; }

        public int StepsInDirection { get; set; }

        public int CompareTo(CityBlock other)
        {
            return this.FScore.CompareTo(other.FScore);
        }

        public override int GetHashCode()
        {
            return this.Location.GetHashCode() ^ this.Direction.GetHashCode() ^ this.StepsInDirection.GetHashCode();
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