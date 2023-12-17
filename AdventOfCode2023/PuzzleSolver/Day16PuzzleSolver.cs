using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day16PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            char[,] map = PuzzleReader.ReadMap(16, test);
            int numEnergized = Solve(map, 0, 0, BeamDirection.Right);
            return numEnergized.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            char[,] map = PuzzleReader.ReadMap(16, test);
            int maxEnergized = Int32.MinValue;

            // Look at top row.
            for (int j = 0; j < map.GetLength(1); j++)
            {
                int numEnergized = Solve(map, 0, j, BeamDirection.Down);
                if (numEnergized > maxEnergized)
                {
                    maxEnergized = numEnergized;
                }
            }

            // Look at bottom row.
            for (int j = 0; j < map.GetLength(1); j++)
            {
                int numEnergized = Solve(map, map.GetLength(0) - 1, j, BeamDirection.Up);
                if (numEnergized > maxEnergized)
                {
                    maxEnergized = numEnergized;
                }
            }

            // Look at leftmost column.
            for (int i = 0; i < map.GetLength(0); i++)
            {
                int numEnergized = Solve(map, i, 0, BeamDirection.Right);
                if (numEnergized > maxEnergized)
                {
                    maxEnergized = numEnergized;
                }
            }

            // Look at rightmost column.
            for (int i = 0; i < map.GetLength(0); i++)
            {
                int numEnergized = Solve(map, i, map.GetLength(1) - 1, BeamDirection.Left);
                if (numEnergized > maxEnergized)
                {
                    maxEnergized = numEnergized;
                }
            }

            return maxEnergized.ToString();
        }

        private static Beam GetNextBeam(Point position, BeamDirection direction, int maxX, int maxY)
        {
            switch (direction)
            {
                case BeamDirection.Left:
                    if (position.Y > 0)
                    {
                        return new Beam { Location = new Point(position.X, position.Y - 1), Direction = direction };
                    }

                    break;

                case BeamDirection.Right:
                    if (position.Y < maxY - 1)
                    {
                        return new Beam { Location = new Point(position.X, position.Y + 1), Direction = direction };
                    }

                    break;

                case BeamDirection.Up:
                    if (position.X > 0)
                    {
                        return new Beam { Location = new Point(position.X - 1, position.Y), Direction = direction };
                    }

                    break;

                case BeamDirection.Down:
                    if (position.X < maxX - 1)
                    {
                        return new Beam { Location = new Point(position.X + 1, position.Y), Direction = direction };
                    }

                    break;

                default:
                    throw new ArgumentException($"Unexpected direction {direction}.");

            }

            return null;
        }

        private static int Solve(char[,] map, int startX, int startY, BeamDirection startDir)
        { 
            int numRows = map.GetLength(0);
            int numCols = map.GetLength(1);

            var beamMap = new char[numRows, numCols];
            var energyMap = new char[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    beamMap[i, j] = energyMap[i, j] = map[i, j];
                }
            }

            energyMap[startX, startY] = '#';

            var queue = new Queue<Beam>();
            var visited = new HashSet<Beam>();

            var beam = new Beam { Location = new Point(startX, startY), Direction = startDir };
            queue.Enqueue(beam);
            
            while (queue.Count > 0)
            {
                Beam current = queue.Dequeue();
                visited.Add(current);
                UpdateBeamMap(beamMap, current);
                var nextDirs = new List<BeamDirection>();
                switch (map[current.Location.X, current.Location.Y])
                {
                    case '.':
                        nextDirs.Add(current.Direction);
                        break;

                    case '/':
                        switch (current.Direction)
                            {
                                case BeamDirection.Right:
                                    nextDirs.Add(BeamDirection.Up);
                                    break;

                                case BeamDirection.Left:
                                    nextDirs.Add(BeamDirection.Down);
                                    break;

                                case BeamDirection.Up:
                                    nextDirs.Add(BeamDirection.Right);
                                    break;

                                case BeamDirection.Down:
                                    nextDirs.Add(BeamDirection.Left);
                                    break;

                                default:
                                    throw new ArgumentException($"Unexpected direction {beam.Direction}.");
                            }

                            break;

                        case '\\':
                            switch (current.Direction)
                            {
                                case BeamDirection.Right:
                                    nextDirs.Add(BeamDirection.Down);
                                    break;

                                case BeamDirection.Left:
                                    nextDirs.Add(BeamDirection.Up);
                                    break;

                                case BeamDirection.Up:
                                    nextDirs.Add(BeamDirection.Left);
                                    break;

                                case BeamDirection.Down:
                                    nextDirs.Add(BeamDirection.Right);
                                    break;

                                default:
                                    throw new ArgumentException($"Unexpected direction {beam.Direction}.");
                            }

                            break;

                        case '|':
                            switch (current.Direction)
                            {
                                case BeamDirection.Right:
                                case BeamDirection.Left:
                                    nextDirs.Add(BeamDirection.Up);
                                    nextDirs.Add(BeamDirection.Down);
                                    break;

                                case BeamDirection.Up:
                                case BeamDirection.Down:
                                    nextDirs.Add(current.Direction);
                                    break;

                                default:
                                    throw new ArgumentException($"Unexpected direction {beam.Direction}.");
                            }

                            break;

                        case '-':
                            switch (current.Direction)
                            {
                                case BeamDirection.Right:
                                case BeamDirection.Left:
                                    nextDirs.Add(current.Direction);
                                    break;

                                case BeamDirection.Up:
                                case BeamDirection.Down:
                                    nextDirs.Add(BeamDirection.Left);
                                    nextDirs.Add(BeamDirection.Right);
                                    break;

                                default:
                                    throw new ArgumentException($"Unexpected direction {beam.Direction}.");
                            }

                            break;

                        default:
                            throw new ArgumentException($"Unexpected character {map[beam.Location.X, beam.Location.Y]}");
                    }

                    foreach (BeamDirection direction in nextDirs)
                    {
                        Beam nextBeam = GetNextBeam(current.Location, direction, numRows, numCols);
                        if (nextBeam != null && !visited.Contains(nextBeam))
                        {
                            queue.Enqueue(nextBeam);
                            energyMap[nextBeam.Location.X, nextBeam.Location.Y] = '#';
                        }
                    }
                }

            /*if (test)
            {
                ConsoleUtilities.PrintMap(beamMap);
                ConsoleUtilities.PrintMap(energyMap);
            }*/

            int numEnergized = visited.Select(x => x.Location).Distinct().Count();
            return numEnergized;
        }

        private static void UpdateBeamMap(char[,] map, Beam beam)
        {
            int i = beam.Location.X;
            int j = beam.Location.Y;

            if (map[i, j] == '/' || map[i, j] == '\\' || map[i, j] == '|' || map[i, j] == '-')
            {
                return;
            }

            if (map[i, j] == 'n')
            {
                return;
            }

            if (char.IsDigit(map[i, j]))
            {
                int num = Int32.Parse(map[i, j].ToString());
                if (num < 9)
                {
                    map[i, j] = (char)(num + 1);
                }
                else
                {
                    map[i, j] = 'n';
                }
            }

            if (map[i, j] == '<' || map[i, j] == '>' || map[i, j] == '|' || map[i, j] == '-')
            {
                map[i, j] = '2';
            }
            else
            {
                map[i, j] = BeamDirectionToChar(beam.Direction);
            }
        }

        private static char BeamDirectionToChar(BeamDirection dir)
        {
            switch (dir)
            {
                case BeamDirection.Left:
                    return '<';

                case BeamDirection.Right:
                    return '>';

                case BeamDirection.Up:
                    return '^';

                case BeamDirection.Down:
                    return 'v';

                default:
                    throw new ArgumentException($"Unexpected direction {dir}.");

            }
        }
    }

    internal class Beam
    {
        public Point Location { get; set; }

        public BeamDirection Direction { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Beam;
            if (other == null)
            {
                return false;
            }

            return this.Location == other.Location && this.Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"({this.Location.X},{this.Location.Y}):{this.Direction}";
        }
    }


    internal enum BeamDirection
    {
        None,

        Up,

        Down,

        Left,

        Right
    }
}
