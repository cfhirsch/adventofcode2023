using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day18PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            var reg = new Regex(@"(\w) (\d+) \(#.+\)", RegexOptions.Compiled);
            var points = new HashSet<Point>();
            var cursor = new Point(0, 0);
            points.Add(cursor);
            foreach (string line in PuzzleReader.ReadLines(18, test))
            {
                var match = reg.Match(line);
                string dir = match.Groups[1].Value;
                int steps = Int32.Parse(match.Groups[2].Value);

                for (int i = 0; i < steps; i++)
                {
                    switch (dir)
                    {
                        case "R":
                            cursor = new Point(cursor.X, cursor.Y + 1);
                            break;

                        case "L":
                            cursor = new Point(cursor.X, cursor.Y - 1);
                            break;

                        case "U":
                            cursor = new Point(cursor.X - 1, cursor.Y);
                            break;

                        case "D":
                            cursor = new Point(cursor.X + 1, cursor.Y);
                            break;

                        default:
                            throw new ArgumentException($"Unexpected direction {dir}.");
                    }

                    points.Add(cursor);
                }
            }

            // Now we need to calculate the volume of the closure of these points.
            var xPoints = points.Select(x => x.X).Distinct();
            var yPoints = points.Select(x => x.Y).Distinct();

            int minX = xPoints.Min();
            int maxX = xPoints.Max();
            int minY = yPoints.Min();
            int maxY = yPoints.Max();

            var map = new char[maxX - minX + 1, maxY - minY + 1];

            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    map[i, j] = '.';
                }
            }

            var internalPoints = new HashSet<Point>();
            foreach (Point pt in points)
            {
                map[pt.X - minX, pt.Y - minY] = '#';
                internalPoints.Add(new Point(pt.X - minX, pt.Y - minY));
            }

            if (test)
            {
                ConsoleUtilities.PrintMap(map);
            }

            var external = new HashSet<Point>();
            for (int i = 0; i < numRows; i++)
            {
                if (map[i, 0] == '.')
                {
                    external.Add(new Point(i, 0));
                }

                if (map[i, numCols - 1] == '.')
                {
                    external.Add(new Point(i, numCols - 1));
                }
            }

            for (int j = 0; j < numCols; j++)
            {
                if (map[0, j] == '.')
                {
                    external.Add(new Point(0, j));
                }

                if (map[numRows - 1, j] == '.')
                {
                    external.Add(new Point(numRows - 1, j));
                }
            }

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    var pt = new Point(i, j);
                    if (!internalPoints.Contains(pt) && !external.Contains(pt))
                    {
                        var queue = new Queue<Point>();
                        var visited = new HashSet<Point>();
                        queue.Enqueue(pt);
                        visited.Add(pt);
                        bool isInternal = true;
                        while (queue.Count > 0)
                        {
                            Point current = queue.Dequeue();
                            if (external.Contains(current))
                            {
                                external.Add(pt);
                                isInternal = false;
                                break;
                            }

                            if (internalPoints.Contains(current))
                            {
                                break;
                            }

                            foreach (Point neighbor in GetNeighbors(map, current))
                            {
                                if (!visited.Contains(neighbor))
                                {
                                    visited.Add(neighbor);
                                    queue.Enqueue(neighbor);
                                }
                            }
                        }

                        if (isInternal)
                        {
                            internalPoints.Add(pt);
                            foreach (Point vpt in visited)
                            {
                                internalPoints.Add(vpt);
                            }
                        }
                    }
                }
            }

            if (test)
            {
                foreach (Point pt in internalPoints)
                {
                    map[pt.X, pt.Y] = '#';
                }

                ConsoleUtilities.PrintMap(map);
            }

            int sum = internalPoints.Count; 
            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Point> GetNeighbors(char[,] map, Point pt)
        {
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            if (pt.X > 0 && map[pt.X - 1, pt.Y] == '.')
            {
                yield return new Point(pt.X - 1, pt.Y);
            }

            if (pt.X < numRows - 1 && map[pt.X + 1, pt.Y] == '.')
            {
                yield return new Point(pt.X + 1, pt.Y);
            }

            if (pt.Y > 0 && map[pt.X, pt.Y - 1] == '.')
            {
                yield return new Point(pt.X, pt.Y - 1);
            }

            if (pt.Y < numCols - 1 && map[pt.X, pt.Y + 1] == '.')
            {
                yield return new Point(pt.X, pt.Y + 1);
            }
        }
    }
}
