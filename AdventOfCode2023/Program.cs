using System;
using AdventOfCode2023.PuzzleSolver;

namespace AdventOfCode2023
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for (int i = 1; i <= 1; i++)
            {
                IPuzzleSolver solver = PuzzleSolverFactory.GetPuzzleSolver(i);
                string part1Solution = solver.SolvePartOne(false);
                Console.WriteLine($"Solution to Day {i}, Part One: {part1Solution}.");
            }

            Console.ReadLine();
        }
    }
}
