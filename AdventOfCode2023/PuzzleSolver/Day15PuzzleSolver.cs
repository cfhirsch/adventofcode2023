using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day15PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            string[] steps = PuzzleReader.ReadLines(15, test).First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            long sum = steps.Sum(x => Hash(x));
            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            Dictionary<int, List<Lens>> boxes = Enumerable.Range(0, 256).ToDictionary(i => i, i => new List<Lens>());
            string[] steps = PuzzleReader.ReadLines(15, test).First().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string step in steps)
            {
                string label = null;
                if (step.EndsWith("-"))
                {
                    label = step.Substring(0, step.IndexOf('-'));
                    int boxNumber = Hash(label);
                    Lens lens = boxes[boxNumber].FirstOrDefault(x => x.Label == label);
                    
                    if (lens != null)
                    {
                        boxes[boxNumber].Remove(lens);
                    }
                }
                else
                {
                    string[] stepParts = step.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                    label = stepParts[0];
                    int focalLength = Int32.Parse(stepParts[1]);
                    int boxNumber = Hash(label);

                    Lens lens = boxes[boxNumber].FirstOrDefault(x => x.Label == label);
                    if (lens != null)
                    {
                        lens.FocalLength = focalLength;
                    }
                    else
                    {
                        boxes[boxNumber].Add(new Lens { Label = label, FocalLength = focalLength });
                    }
                }
            }

            long sum = 0;
            foreach (KeyValuePair<int, List<Lens>> kvp in boxes)
            {
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    sum += (kvp.Key + 1) * (i + 1) * kvp.Value[i].FocalLength;
                }
            }

            return sum.ToString();
        }

        private static int Hash(string s)
        {
            int hash = 0;
            foreach (char ch in s)
            {
                hash += ch;
                hash *= 17;
                hash %= 256;
            }

            return hash;
        }
    }

    internal class Lens : IEquatable<Lens>
    {
        public string Label { get; set; }

        public int FocalLength { get; set; }

        public bool Equals(Lens other)
        {
            if (other == null)
            {
                return false;
            }


            return this.Label == other.Label && this.FocalLength == other.FocalLength;
        }
    }
}
