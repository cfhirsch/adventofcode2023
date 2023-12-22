using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
            
            var steps = new List<Step>();
            foreach (string line in PuzzleReader.ReadLines(18, test))
            {
                var match = reg.Match(line);
                string dir = match.Groups[1].Value;
                int count = Int32.Parse(match.Groups[2].Value);

                steps.Add(new Step { Direction = dir, Count = count });
            }

            long area = Area(steps);
            return area.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            var reg = new Regex(@"(\w) (\d+) \(#(.+)\)", RegexOptions.Compiled);

            var steps = new List<Step>();
            foreach (string line in PuzzleReader.ReadLines(18, test))
            {
                var match = reg.Match(line);
                string hexDigit = match.Groups[3].Value;

                long count = Int64.Parse(hexDigit.Substring(0, 5), NumberStyles.HexNumber);

                string dir;
                switch (hexDigit[5])
                {
                    case '0':
                        dir = "R";
                        break;

                    case '1':
                        dir = "D";
                        break;

                    case '2':
                        dir = "L";
                        break;

                    case '3':
                        dir = "U";
                        break;

                    default:
                        throw new ArgumentException($"Unexpected dir {hexDigit[5]}");
                }

                steps.Add(new Step { Direction = dir, Count = count });
            }

            long area = Area(steps);
            return area.ToString();
        }


        /// <summary>
        /// Solution cribbed from https://github.com/mnvr/advent-of-code-2023/blob/main/18.swift
        /// </summary>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static long Area(IEnumerable<Step> steps)
        {
            long px = 0, py = 0, s = 0;
            foreach (Step step in steps)
            {
                long x, y;
                switch (step.Direction)
                {
                    case "R":
                        (x, y) = (px + step.Count, py);
                        break;

                    case "L":
                        (x, y) = (px - step.Count, py);
                        break;

                    case "U":
                        (x, y) = (px, py - step.Count);
                        break;

                    case "D":
                        (x, y) = (px, py + step.Count);
                        break;

                    default:
                        throw new ArgumentException($"Unexpected direction {step.Direction}.");
                }

                s += (py + y) * (px - x);
                s += step.Count;
                (px, py) = (x, y);            
            }

            return (Math.Abs(s) / 2) + 1; 
        }
    }

    internal struct Step
    { 
        public string Direction { get; set; }
        
        public long Count { get; set; }
    }
}
