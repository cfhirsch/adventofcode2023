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

                default:
                    throw new ArgumentException($"Unsupported day {day}.");
            }
        }
    }
}
