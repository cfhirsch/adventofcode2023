using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.NetworkInformation;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day24PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            long min = test ? 7 : 200000000000000;
            long max = test ? 27 : 400000000000000;
            long minX = min, minY = min, maxX = max, maxY = max;

            var hailstones = new List<HailStone>();
            foreach (string line in PuzzleReader.ReadLines(24, test))
            {
                string[] lineParts = line.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                string[] positions = lineParts[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string[] velocities = lineParts[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                hailstones.Add(new HailStone
                {
                    Position = new Point3D(Int64.Parse(positions[0]), Int64.Parse(positions[1]), Int64.Parse(positions[2])),
                    Velocity = new Point3D(Int64.Parse(velocities[0]), Int64.Parse(velocities[1]), Int64.Parse(velocities[2]))
                });
            }

            int interesectionCount = 0;
            for (int i = 0; i < hailstones.Count - 1; i++)
            {
                for (int j = i + 1; j < hailstones.Count; j++)
                {
                    HailStone h1 = hailstones[i];
                    HailStone h2 = hailstones[j];

                    if (test)
                    {
                        Console.WriteLine($"Hailstone A: {h1}.");
                        Console.WriteLine($"Hailstone B: {h2}.");
                    }

                    double a_0_0 = h1.Velocity.X;
                    double a_0_1 = -1 * h2.Velocity.X;
                    double a_1_0 = h1.Velocity.Y;
                    double a_1_1 = -1 * h2.Velocity.Y;

                    // detA = ad - bc
                    double determinant = a_0_0 * a_1_1 - (a_0_1 * a_1_0);

                    if (determinant == 0)
                    { 
                        if (test)
                        {
                            Console.WriteLine("Hailstones's paths are paralllel; they never interesect.");
                        }

                        continue;
                    }
                    
                    double a_inv_0_0 = a_1_1 / determinant;
                    double a_inv_0_1 = (-1 * a_0_1) / determinant;
                    double a_inv_1_0 = (-1 * a_1_0) / determinant;
                    double a_inv_1_1 = a_0_0 / determinant;

                    double t1 = (a_inv_0_0 * (h2.Position.X - h1.Position.X)) + (a_inv_0_1 * (h2.Position.Y - h1.Position.Y));
                    double t2 = (a_inv_1_0 * (h2.Position.X - h1.Position.X)) + (a_inv_1_1 * (h2.Position.Y - h1.Position.Y));

                    if (t1 < 0)
                    {
                        if (t2 < 0)
                        {
                            if (test)
                            {
                                Console.WriteLine("Hailstones' paths crossed in the past for both hailstones.");
                            }
                        }
                        else
                        {
                            if (test)
                            {
                                Console.WriteLine("Hailstones' paths crossed in the past for hailstone A.");
                            }
                        }

                        continue;
                    }

                    if (t2 < 0)
                    {
                        if (test)
                        {
                            Console.WriteLine("Hailstones' paths crossed in the past for hailstone B.");
                        }

                        continue;
                    }

                    double x1 = h1.Position.X + t1 * h1.Velocity.X;
                    
                    double y1 = h1.Position.Y + t1 * h1.Velocity.Y;
                    
                    if (minX <= x1 && x1 <= maxX && minY <= y1 && y1 <= maxY)
                    {
                        interesectionCount++;

                        if (test)
                        {
                            Console.WriteLine($"Hailstone's paths will cross inside the test area (at (x={x1},y={y1}).");
                        }
                    }
                    else
                    {
                        if (test)
                        {
                            Console.WriteLine($"Hailstone's paths will cross outside the test area (at (x={x1},y={y1}).");
                        }
                    }
                }
            }

            return interesectionCount.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }
    }

    internal class HailStone
    {
        public Point3D Position { get; set; }

        public Point3D Velocity { get; set; }

        public override string ToString()
        {
            return $"{this.Position.X}, {this.Position.Y}, {this.Position.Z} @ {this.Velocity.X}, {this.Velocity.Y}, {this.Velocity.Z}";
        }
    }
}
