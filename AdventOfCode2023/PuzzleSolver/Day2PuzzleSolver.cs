using System;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day2PuzzleSolver : IPuzzleSolver
    {
        private static Regex sampleReg = new Regex(@"(\d+) (\w+)", RegexOptions.Compiled);

        public string SolvePartOne(bool test = false)
        {
            int numRed = 12;
            int numGreen = 13;
            int numBlue = 14;

            int gameID = 1;
            int sum = 0;
            foreach (string line in PuzzleReader.ReadLines(2, test))
            {
                string game = line.Substring("Game 1: ".Length);
                string[] turns = game.Split(';');

                bool possible = true;
                foreach (string turn in turns)
                {
                    string[] samples = turn.Split(',');
                    foreach (string sample in samples)
                    {
                        Match match = sampleReg.Match(sample);
                        int numCubes = Int32.Parse(match.Groups[1].Value);
                        string color = match.Groups[2].Value;

                        switch (color)
                        {
                            case "red":
                                if (numCubes > numRed)
                                {
                                    possible = false;
                                }

                                break;

                            case "green":
                                if (numCubes > numGreen)
                                {
                                    possible = false;
                                }

                                break;

                            case "blue":
                                if (numCubes > numBlue)
                                {
                                    possible = false;
                                }

                                break;

                            default:
                                throw new ArgumentException($"Unexpected color {color}.");
                        }

                        if (!possible)
                        {
                            break;
                        }
                    }
                }

                if (possible)
                {
                    sum += gameID;
                }

                gameID++;
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }
    }
}
