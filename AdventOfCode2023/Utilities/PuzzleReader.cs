using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2023.Utilities
{
    internal static class PuzzleReader
    {
        public static IEnumerable<string> ReadLines(int day, bool isTest = false)
        {
            string fileName = isTest ? $"Dec{day}_test.txt" : $"Dec{day}.txt";
            using (var stream = new FileStream($@"c:\docs\AdventOfCode2023\{fileName}", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return reader.ReadLine();
                    }
                }
            }
        }

        public static string ReadAll(int day, bool isTest = false)
        {
            string fileName = isTest ? $"Dec{day}_test.txt" : $"Dec{day}.txt";
            using (var stream = new FileStream($@"c:\docs\AdventOfCode2023\{fileName}", FileMode.Open))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

                public static char[,] ReadMap(int day, bool isTest = false)
        {
            List<string> lines = ReadLines(day, isTest).ToList();
            var map = new char[lines.Count, lines[0].Length];
            for (int i = 0; i < lines.Count; i++) 
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    map[i, j] = lines[i][j];
                }
            }

            return map;
        }
    }
}
