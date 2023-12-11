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

                default:
                    throw new ArgumentException($"Unsupported day {day}.");
            }
        }
    }
}
