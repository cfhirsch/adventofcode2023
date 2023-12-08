using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using AdventOfCode2023.Utilities;

namespace AdventOfCode2023.PuzzleSolver
{
    internal class Day7PuzzleSolver : IPuzzleSolver
    {
        public string SolvePartOne(bool test = false)
        {
            return Solve(test, isPartTwo: false);
        }

        public string SolvePartTwo(bool test = false)
        {
            return Solve(test, isPartTwo: true);
        }

        private static string Solve(bool test, bool isPartTwo)
        {
            var hands = new List<Hand>();

            foreach (string line in PuzzleReader.ReadLines(7, test))
            {
                string[] lineParts = line.Split(' ');
                hands.Add(new Hand(lineParts[0], Int32.Parse(lineParts[1]), isPartTwo));
            }

            hands.Sort();

            long sum = 0;
            for (int i = 1; i <= hands.Count; i++)
            {
                if (isPartTwo)
                {
                    Console.WriteLine($"{hands[i - 1]}:{hands[i - 1].HandType}");
                }
                sum += i * hands[i - 1].Bid;
            }

            return sum.ToString();
        }
    }

    internal class Hand : IComparable<Hand>
    {
        private char[] hand;
        private Dictionary<char, int> cardCount;
        private bool isPartTwo;

        public Hand(string handStr, long bid, bool isPartTwo = false)
        {
            this.hand = handStr.ToCharArray();
            this.cardCount =
                this.hand.GroupBy(c => c).ToDictionary
                (x => x.Key, x => x.Count());

            this.Bid = bid;
            this.isPartTwo = isPartTwo;
        }

        public long Bid { get; }

        public HandType HandType
        {
            get
            {
                if (isPartTwo && this.cardCount.ContainsKey('J'))
                {
                    int jokerCount = this.cardCount['J'];

                    // Number of ranks in the hand that are not 'J'.
                    var nonJokerRanks = this.cardCount.Where(x => x.Key != 'J');

                    int numNonJokerRanks = nonJokerRanks.Count();

                    // The max number of cards for a rank that is not a Joker.
                    int maxNonJoker = 0;

                    if (nonJokerRanks.Any())
                    {
                        maxNonJoker = nonJokerRanks.OrderByDescending(x => x.Value).First().Value;
                    }

                    switch (jokerCount)
                    {
                        case 5:
                        case 4:
                            // We either have all jokers, or 4 jokers and one other card.
                            return HandType.FiveOfAKind;

                        case 3:
                            // We either have JJJXX or JJJXY. 
                            // In the former we can get five of a kind, in the latter four of a kind.
                            return numNonJokerRanks == 1 ? HandType.FiveOfAKind : HandType.FourOfAKind;

                        case 2:
                            switch (numNonJokerRanks)
                            {
                                case 1:
                                    // JJXXX
                                    return HandType.FiveOfAKind;

                                case 2:
                                    // JJXXY
                                    return HandType.FourOfAKind;

                                case 3:
                                    // JJXYZ
                                    return HandType.ThreeOfAKind;

                                default:
                                    throw new ArgumentException($"Unexpected number of non-joker ranks {numNonJokerRanks}.");
                            }

                        case 1:
                            switch (numNonJokerRanks)
                            {
                                case 1:
                                    // JXXXX
                                    return HandType.FiveOfAKind;

                                case 2:
                                    if (maxNonJoker == 3)
                                    {
                                        // JXXXY
                                        return HandType.FourOfAKind;
                                    }
                                    else
                                    {
                                        // JXXYY
                                        return HandType.FullHouse;
                                    }

                                case 3:
                                    // JXXYZ
                                    return HandType.ThreeOfAKind;

                                case 4:
                                    // JXYZW
                                    return HandType.OnePair;

                                default:
                                    throw new ArgumentException($"Unexpected number of non-joker ranks {numNonJokerRanks}.");
                            }
                    }
                }
                    
                switch (this.cardCount.Count())
                {
                    case 1:
                        return HandType.FiveOfAKind;

                    case 2:
                        if (this.cardCount.Any(k => k.Value == 4))
                        {
                            return HandType.FourOfAKind;
                        }
                        else
                        {
                            return HandType.FullHouse;
                        }

                    case 3:
                        if (cardCount.Select(k => k.Value).Max() == 3)
                        {
                            return HandType.ThreeOfAKind;
                        }

                        return HandType.TwoPair;

                    case 4:
                        return HandType.OnePair;

                    default:
                        return HandType.HighCard;
                }
            }
        }

        public int CompareTo(Hand other)
        {
            if (this.HandType < other.HandType)
            {
                return -1;
            }

            if (this.HandType > other.HandType)
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
            return $"{string.Join("", this.hand)}";
        }

        private int CompareCards(char card1, char card2)
        {
            return CardToInt(card1).CompareTo(CardToInt(card2));
        }

        private int CardToInt(char ch)
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
                    return this.isPartTwo ? 1 : 11;

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

    internal enum HandType
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
