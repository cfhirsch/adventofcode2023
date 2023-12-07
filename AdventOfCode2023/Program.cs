using System;
using AdventOfCode2023.PuzzleSolver;

namespace AdventOfCode2023
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 7; i <= 7; i++)
            {
                try
                {
                    IPuzzleSolver solver = PuzzleSolverFactory.GetPuzzleSolver(i);

                    try
                    {
                        string part1Solution = solver.SolvePartOne(test: false);
                        Console.WriteLine($"Solution to Day {i}, Part One: {part1Solution}.");
                    }
                    catch (NotImplementedException)
                    {
                        Console.WriteLine($"Solver not implemented for day {i}, part one.");
                    }

                    try
                    {
                        string part2Solution = solver.SolvePartTwo(test: false);
                        Console.WriteLine($"Solution to Day {i}, Part Two: {part2Solution}.");
                    }
                    catch (NotImplementedException)
                    {
                        Console.WriteLine($"Solver not implemented for day {i}, part two.");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine($"Puzzle solver not implemented for day {i}.");
                }
            }

            Console.ReadLine();
        }
    }
}
