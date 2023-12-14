using System;

namespace AdventOfCode2023.Utilities
{
    internal static class ConsoleUtilities
    {
        public static void PrintMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();

        }

    }
}
