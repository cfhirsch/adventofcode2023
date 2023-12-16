using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day16PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            char[,] map = PuzzleReader.ReadMap(16, test);
            
            var beams = new List<Beam>
            {
                new Beam { Location = new Point(0, 0), Direction = BeamDirection.Right }
            };

            int numEnergized = 0;
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

            energyMap[0, 0] = '#';

            while (beams.Any())
            {
                //ConsoleUtilities.PrintMap(beamMap);
                //ConsoleUtilities.PrintMap(energyMap);
                //Console.Read();

                var nextBeams = new List<Beam>();
                foreach (Beam beam in beams)
                {
                    UpdateBeamMap(beamMap, beam);
                    switch (beam.Direction)
                    {
                        case BeamDirection.Right:
                            if (beam.Location.Y < numCols - 1)
                            {
                                var nextPoint = new Point(beam.Location.X, beam.Location.Y + 1);
                                var nextDirs = new List<BeamDirection>();
                                char nextCh = map[beam.Location.X, beam.Location.Y + 1];
                                switch (nextCh)
                                {
                                    case '.':
                                    case '-':
                                        nextDirs.Add(BeamDirection.Right);
                                        break;

                                    case '/':
                                        nextDirs.Add(BeamDirection.Up);
                                        break;

                                    case '\\':
                                        nextDirs.Add(BeamDirection.Down);
                                        break;

                                    case '|':
                                        nextDirs.Add(BeamDirection.Up);
                                        nextDirs.Add(BeamDirection.Down);
                                        break;

                                    default:
                                        throw new ArgumentException($"Unexpected char {nextCh}.");
                                }

                                foreach (BeamDirection dir in nextDirs)
                                {
                                    nextBeams.Add(new Beam { Direction = dir, Location = nextPoint });
                                }
                            }

                            break;

                        case BeamDirection.Left:

                            if (beam.Location.Y > 0)
                            {
                                var nextPoint = new Point(beam.Location.X, beam.Location.Y - 1);
                                var nextDirs = new List<BeamDirection>();
                                char nextCh = map[beam.Location.X, beam.Location.Y - 1];
                                switch (nextCh)
                                {
                                    case '.':
                                    case '-':
                                        nextDirs.Add(BeamDirection.Left);
                                        break;

                                    case '/':
                                        nextDirs.Add(BeamDirection.Down);
                                        break;

                                    case '\\':
                                        nextDirs.Add(BeamDirection.Up);
                                        break;

                                    case '|':
                                        nextDirs.Add(BeamDirection.Up);
                                        nextDirs.Add(BeamDirection.Down);
                                        break;

                                    default:
                                        throw new ArgumentException($"Unexpected char {nextCh}.");
                                }

                                foreach (BeamDirection dir in nextDirs)
                                {
                                    nextBeams.Add(new Beam { Direction = dir, Location = nextPoint });
                                }
                            }

                            break;

                        case BeamDirection.Up:

                            if (beam.Location.X > 0)
                            {
                                var nextPoint = new Point(beam.Location.X - 1, beam.Location.Y);
                                var nextDirs = new List<BeamDirection>();
                                char nextCh = map[beam.Location.X - 1, beam.Location.Y];
                                switch (nextCh)
                                {
                                    case '.':
                                    case '|':
                                        nextDirs.Add(BeamDirection.Up);
                                        break;

                                    case '/':
                                        nextDirs.Add(BeamDirection.Right);
                                        break;

                                    case '\\':
                                        nextDirs.Add(BeamDirection.Left);
                                        break;

                                    case '-':
                                        nextDirs.Add(BeamDirection.Left);
                                        nextDirs.Add(BeamDirection.Right);
                                        break;

                                    default:
                                        throw new ArgumentException($"Unexpected char {nextCh}.");
                                }

                                foreach (BeamDirection dir in nextDirs)
                                {
                                    nextBeams.Add(new Beam { Direction = dir, Location = nextPoint });
                                }
                            }
                            break;

                            case BeamDirection.Down:

                                if (beam.Location.X < numRows - 1)
                                {
                                    var nextPoint = new Point(beam.Location.X + 1, beam.Location.Y);
                                    var nextDirs = new List<BeamDirection>();
                                    char nextCh = map[beam.Location.X + 1, beam.Location.Y];
                                    switch (nextCh)
                                    {
                                        case '.':
                                        case '|':
                                            nextDirs.Add(BeamDirection.Down);
                                            break;

                                        case '/':
                                            nextDirs.Add(BeamDirection.Left);
                                            break;

                                        case '\\':
                                            nextDirs.Add(BeamDirection.Right);
                                            break;

                                        case '-':
                                            nextDirs.Add(BeamDirection.Left);
                                            nextDirs.Add(BeamDirection.Right);
                                            break;

                                        default:
                                            throw new ArgumentException($"Unexpected char {nextCh}.");
                                    }

                                    foreach (BeamDirection dir in nextDirs)
                                    {
                                        nextBeams.Add(new Beam { Direction = dir, Location = nextPoint });
                                    }
                                }

                                break;

                        default:
                            throw new ArgumentException($"Unexpected direction {beam.Direction}");

                    }
                }

                beams = nextBeams;

                foreach (Beam beam in beams)
                {
                    energyMap[beam.Location.X, beam.Location.Y] = '#';
                }

                int nextEnergized = 0;
                for (int i = 0; i < numRows; i++)
                {
                    for (int j = 0; j < numCols; j++)
                    {
                        if (energyMap[i, j] == '#')
                        {
                            nextEnergized++;
                        }
                    }
                }

                if (nextEnergized == numEnergized)
                {
                    break;
                }
                else
                {
                    numEnergized = nextEnergized;
                }

            }

            ConsoleUtilities.PrintMap(beamMap);
            ConsoleUtilities.PrintMap(energyMap);

            return numEnergized.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }

        private static void UpdateBeamMap(char[,] map, Beam beam)
        {
            int i = beam.Location.X;
            int j = beam.Location.Y;

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

    internal struct Beam
    {
        public Point Location { get; set; }

        public BeamDirection Direction { get; set; }
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
