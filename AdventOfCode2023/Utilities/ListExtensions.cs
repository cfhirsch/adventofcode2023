using System.Collections.Generic;

namespace AdventOfCode2023.Utilities
{
    internal static class ListExtensions
    {
        public static char[,] ToCharArray(this List<string> lineList)
        {
            int numRows = lineList.Count;
            int numCols = lineList[0].Length;

            char[,] map = new char[numRows, numCols];

            for (int x = 0; x < numRows; x++)
            {
                for (int y = 0; y < numCols; y++)
                {
                    map[x, y] = lineList[x][y];
                }
            }

            return map;
        }
    }
}
