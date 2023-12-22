using System;

namespace AdventOfCode2023.PuzzleSolver
{
    internal static class PuzzleSolverFactory
    {
        public static IPuzzleSolver GetPuzzleSolver(int day)
        {
            switch (day)
            {
                case 1:
                    return new Day1PuzzleSolver();

                case 2:
                    return new Day2PuzzleSolver();

                case 3:
                    return new Day3PuzzleSolver();

                case 4:
                    return new Day4PuzzleSolver();

                case 5:
                    return new Day5PuzzleSolver();

                case 6:
                    return new Day6PuzzleSolver();

                case 7:
                    return new Day7PuzzleSolver();

                case 8:
                    return new Day8PuzzleSolver();

                case 9:
                    return new Day9PuzzleSolver();

                case 10:
                    return new Day10PuzzlerSolver();

                case 11:
                    return new Day11PuzzleSolver();

                case 12:
                    return new Day12PuzzleSolver();

                case 13:
                    return new Day13PuzzleSolver();

                case 14:
                    return new Day14PuzzleSolver();

                case 15:
                    return new Day15PuzzleSolver();

                case 16:
                    return new Day16PuzzleSolver();

                case 17:
                    return new Day17PuzzleSolver();

                case 18:
                    return new Day18PuzzleSolver();

                case 19:
                    return new Day19PuzzleSolver();

                case 20:
                    return new Day20PuzzleSolver();

                case 21:
                    return new Day21PuzzleSolver();

                case 22:
                    return new Day22PuzzleSolver();

                default:
                    throw new ArgumentException($"Unsupported day {day}.");
            }
        }
    }
}
