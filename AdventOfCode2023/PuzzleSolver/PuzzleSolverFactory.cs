﻿using System;

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

                default:
                    throw new ArgumentException($"Unsupported day {day}.");
            }
        }
    }
}
