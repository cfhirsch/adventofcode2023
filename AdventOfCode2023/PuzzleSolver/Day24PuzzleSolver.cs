using System;
using System.Collections.Generic;
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

        // Cribbed the answer from [here](https://topaz.github.io/paste/#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).
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
