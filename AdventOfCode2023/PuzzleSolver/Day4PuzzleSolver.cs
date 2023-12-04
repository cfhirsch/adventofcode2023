using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day4PuzzleSolver : IPuzzleSolver
    {
        // Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53

        public string SolvePartOne(bool test = false)
        {
            long sum = 0;
            foreach (string line in PuzzleReader.ReadLines(4, test))
            {
                string card = line.Substring(line.IndexOf(':') + 1);
                string[] cardParts = card.Split('|');
                List<int> winningNumbers = cardParts[0].Trim().Split(
                    new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();

                List<int> cardNumbers = cardParts[1].Trim().Split(
                    new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();

                int exponent = cardNumbers.Where(n => winningNumbers.Contains(n)).Count() - 1;
                
                if (exponent >= 0)
                {
                    sum += (long) Math.Pow(2, exponent);
                }
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            long sum = 0;
            int cardNumber = 1;
            var cardCount = new Dictionary<int, long>();
            foreach (string line in PuzzleReader.ReadLines(4, test))
            {
                if (!cardCount.ContainsKey(cardNumber))
                {
                    cardCount[cardNumber] = 1;
                }

                long numOfCurrentCard = 1;
                if (cardCount.ContainsKey(cardNumber))
                {
                    numOfCurrentCard = cardCount[cardNumber];
                }

                string card = line.Substring(line.IndexOf(':') + 1);
                string[] cardParts = card.Split('|');
                List<int> winningNumbers = cardParts[0].Trim().Split(
                    new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();

                List<int> cardNumbers = cardParts[1].Trim().Split(
                    new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();

                int winCount = cardNumbers.Where(n => winningNumbers.Contains(n)).Count();

                for (int i = cardNumber + 1; i <= cardNumber + winCount; i++)
                {
                    if (!cardCount.ContainsKey(i))
                    {
                        cardCount.Add(i, 1);
                    }

                    cardCount[i] += numOfCurrentCard;
                }

                cardNumber++;
            }

            return cardCount.Values.Sum().ToString();
        }
    }
}
