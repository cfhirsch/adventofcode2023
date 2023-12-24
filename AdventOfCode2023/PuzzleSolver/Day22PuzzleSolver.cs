using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day22PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var bricks = new List<Brick>();
            int j = 0;
            foreach (string line in PuzzleReader.ReadLines(22, test))
            {
                string[] points = line.Split(new[] { '~' }, StringSplitOptions.None);

                var startParts = points[0].Split(new[] { ',' }, StringSplitOptions.None);
                var endParts = points[1].Split(new[] { ',' }, StringSplitOptions.None);
                var start = new Point3D(Int32.Parse(startParts[0]), Int32.Parse(startParts[1]), Int32.Parse(startParts[2]));
                var end = new Point3D(Int32.Parse(endParts[0]), Int32.Parse(endParts[1]), Int32.Parse(endParts[2]));

                string label = ((char)((int)'A' + j)).ToString();
                bricks.Add(new Brick(label, start, end));
                j++;
            }

            // Order the bricks by z ccordinate.
            bricks = bricks.OrderBy(b => Math.Min(b.Start.Z, b.End.Z)).ToList();

            // supportsDict[b] will contain the set of bricks that support b.
            var supportsDict = new Dictionary<Brick, HashSet<Brick>>();

            bool moved = true;
            while (moved)
            {
                moved = false;
                var newBricks = new List<Brick>();

                for (int i = 0; i < bricks.Count; i++)
                {
                    Brick current = bricks[i];
                    Point3D start = current.Start;
                    if (Math.Min(current.Start.Z, current.End.Z) > 1)
                    {
                        bool adjacent = false;
                        foreach (Brick adjBrick in newBricks.Where(b => Adjacent(current, b)))
                        {
                            adjacent = true;
                            if (!supportsDict.ContainsKey(current))
                            {
                                supportsDict.Add(current, new HashSet<Brick>());
                            }

                            supportsDict[current].Add(adjBrick);
                        }

                        if (!adjacent)
                        {
                            current.Fall();
                            moved = true;
                        }
                    }

                    newBricks.Add(current);
                }

                bricks = newBricks;
                bricks = bricks.OrderBy(b => Math.Min(b.Start.Z, b.End.Z)).ToList();
            }

            int numBricks = 0;
            foreach (Brick brick in bricks)
            {
                if (supportsDict.Any(k => k.Value.Count == 1 && k.Value.Contains(brick)))
                {
                    if (test)
                    {
                        Console.WriteLine($"Brick {brick.Label} supports bricks and cannot be removed.");
                    }
                }
                else
                {
                    if (test)
                    {
                        Console.WriteLine($"Brick {brick.Label} can be removed.");
                    }

                    numBricks++;
                }
            }

            return numBricks.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static bool Adjacent(Brick b1, Brick b2)
        {
            // Brick b1 is lying on top of brick b2 iff:
            // minZ of b1 = max Z of b2 + 1 AND
            // b1's X range intersects B2's X range AND b1's y range intersects B2's y range.
            if (Math.Min(b1.Start.Z, b1.End.Z) == Math.Max(b2.Start.Z, b2.End.Z) + 1)
            {
                return Intersects(b1.Start.X, b1.End.X, b2.Start.X, b2.End.X) &&
                       Intersects(b1.Start.Y, b1.End.Y, b2.Start.Y, b2.End.Y);
            }

            return false;
        }

        private static bool Intersects(long start1, long end1, long start2, long end2)
        {
            if (start1 >= start2 && start1 <= end2)
            {
                return true;
            }

            if (end1 >= start2 && end1 <= end2)
            {
                return true;
            }

            if (start2 >= start1 && start2 <= end1)
            {
                return true;
            }

            if (end2 >= start1 && end2 <= end1)
            {
                return true;
            }

            return false;
        }
    }

    internal struct Brick : IEquatable<Brick>
    {
        public Brick(string label, Point3D start, Point3D end)
        {
            this.Label = label;
            this.Start = start;
            this.End = end;
        }

        public string Label { get; private set; }

        public Point3D Start { get; private set; }

        public Point3D End { get; private set; }

        public bool Equals(Brick other)
        {
            return this.Start.Equals(other.Start) && this.End.Equals(other.End);
        }

        public void Fall()
        {
            this.Start = new Point3D(this.Start.X, this.Start.Y, this.Start.Z - 1);
            this.End = new Point3D(this.End.X, this.End.Y, this.End.Z - 1);
        }

        public override int GetHashCode()
        {
            return this.Start.GetHashCode() ^ this.End.GetHashCode();
        }
    }

    internal struct Point3D : IEquatable<Point3D>
    {
        public Point3D(long x, long y, long z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public long X { get; private set; }

        public long Y { get; private set; }

        public long Z { get; private set; }

        public bool Equals(Point3D other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }
    }
}
