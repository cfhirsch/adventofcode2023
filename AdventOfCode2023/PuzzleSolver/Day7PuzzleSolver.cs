using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day7PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            //var h1 = new Hand("TK8A9", 1);
            //var h2 = new Hand("TK82A", 2);
            //int r = h1.CompareTo(h2);

            var hands = new List<Hand>();

            foreach (string line in PuzzleReader.ReadLines(7, test))
            {
                string[] lineParts = line.Split(' ');
                hands.Add(new Hand(lineParts[0], Int32.Parse(lineParts[1])));
            }

            hands.Sort();

            long sum = 0;
            for (int i = 1; i <= hands.Count; i++)
            {
                Console.WriteLine(hands[i - 1]);
                sum += i * hands[i - 1].Bid;
            }

            return sum.ToString();
        }

        public string SolvePartTwo(bool test = false)
        {
            throw new NotImplementedException();
        }
    }

    internal class Hand : IComparable<Hand>
    {
        private char[] hand;
        private Dictionary<char, int> cardCount;

        public Hand(string handStr, long bid)
        {
            this.hand = handStr.ToCharArray();
            this.cardCount =
                this.hand.GroupBy(c => c).ToDictionary
                (x => x.Key, x => x.Count());

            this.Bid = bid;
        }

        public long Bid { get; }

        public Rank Rank
        {
            get
            {
                switch (this.cardCount.Count())
                {
                    case 1:
                        return Rank.FiveOfAKind;

                    case 2:
                        if (this.cardCount.Any(k => k.Value == 4))
                        {
                            return Rank.FourOfAKind;
                        }
                        else
                        {
                            return Rank.FullHouse;
                        }

                    case 3:
                        if (cardCount.Select(k => k.Value).Max() == 3)
                        {
                            return Rank.ThreeOfAKind;
                        }

                        return Rank.TwoPair;

                    case 4:
                        return Rank.OnePair;

                    default:
                        return Rank.HighCard;
                }
            }
        }

        public int CompareTo(Hand other)
        {
            if (this.Rank < other.Rank)
            {
                return -1;
            }

            if (this.Rank > other.Rank)
            {
                return 1;
            }

            for (int i = 0; i < this.hand.Length; i++)
            {
                int result = CompareCards(this.hand[i], other.hand[i]);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return $"{string.Join("", this.hand)}, {this.Rank}";
        }

        private static int CompareCards(char card1, char card2)
        {
            return CardToInt(card1).CompareTo(CardToInt(card2));
        }

        private static int CardToInt(char ch)
        {
            if (char.IsDigit(ch))
            {
                return Int32.Parse(ch.ToString());
            }

            switch (ch)
            {
                case 'T':
                    return 10;

                case 'J':
                    return 11;

                case 'Q':
                    return 12;

                case 'K':
                    return 13;

                case 'A':
                    return 14;

                default:
                    throw new ArgumentException($"Unexpected card {ch}.");
            }
        }
    }

    internal enum Rank
    {
        None = 0,
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5, 
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1,
    }
}
