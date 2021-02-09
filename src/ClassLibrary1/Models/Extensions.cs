using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace ClassLibrary1.Models
{
    public static class Extensions
    {
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            var avg = values.Average();

            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }

        public static int LowValue(this Rank rank)
        {
            switch (rank)
            {
                case Rank.Ace:
                    return 1;

                case Rank.Two:
                    return 2;

                case Rank.Three:
                    return 3;

                case Rank.Four:
                    return 4;

                case Rank.Five:
                    return 5;

                case Rank.Six:
                    return 6;

                case Rank.Seven:
                    return 7;

                case Rank.Eight:
                    return 8;

                case Rank.Nine:
                    return 9;

                case Rank.Ten:
                case Rank.Jack:
                case Rank.Queen:
                case Rank.King:
                    return 10;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static int HighValue(this Rank rank)
        {
            switch (rank)
            {
                case Rank.Ace:
                    return 11;

                case Rank.Two:
                    return 2;

                case Rank.Three:
                    return 3;

                case Rank.Four:
                    return 4;

                case Rank.Five:
                    return 5;

                case Rank.Six:
                    return 6;

                case Rank.Seven:
                    return 7;

                case Rank.Eight:
                    return 8;

                case Rank.Nine:
                    return 9;

                case Rank.Ten:
                case Rank.Jack:
                case Rank.Queen:
                case Rank.King:
                    return 10;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static string Description(this Rank rank)
        {
            switch (rank)
            {
                case Rank.Ace:
                    return "A";

                case Rank.Two:
                    return "2";

                case Rank.Three:
                    return "3";

                case Rank.Four:
                    return "4";

                case Rank.Five:
                    return "5";

                case Rank.Six:
                    return "6";

                case Rank.Seven:
                    return "7";

                case Rank.Eight:
                    return "8";

                case Rank.Nine:
                    return "9";

                case Rank.Ten:
                    return "10";

                case Rank.Jack:
                    return "J";

                case Rank.Queen:
                    return "Q";

                case Rank.King:
                    return "K";

                default:
                    throw new InvalidOperationException();
            }
        }

        public static string Description(this Suit suit)
        {
            switch (suit)
            {
                case Suit.Spades:
                    return "♠";

                case Suit.Diamonds:
                    return "♦";

                case Suit.Clubs:
                    return "♣";

                case Suit.Hearts:
                    return "♥";

                default:
                    throw new InvalidOperationException();
            }
        }

        public static string Description(this Action action)
        {
            switch (action)
            {
                case Action.Stand:
                    return "S";

                case Action.Hit:
                    return "H";

                case Action.Double:
                    return "D";

                case Action.Split:
                    return "P";

                default:
                    throw new InvalidOperationException();
            }
        }

        public static Color Color(this Action action)
        {
            switch (action)
            {
                case Action.Stand:
                    return Colors.Red;

                case Action.Hit:
                    return Colors.LightGreen;

                case Action.Double:
                    return Colors.Yellow;

                case Action.Split:
                    return Colors.MediumPurple;

                default:
                    throw new InvalidOperationException();
            }
        }

        public static int GetActionIndex(this IStrategy strategy, Hand playerHand, Hand dealerHand)
        {
            const int COLUMNS_PER_ROW = 10;

            var row = GetActionRow(playerHand);
            var column = GetActionColumn(dealerHand);
            var index = row * COLUMNS_PER_ROW  + column;

            return index;
        }

        private static int GetActionRow(Hand playerHand)
        {
            if (playerHand.HasPair)
            {
                switch (playerHand.Cards[0].Rank)
                {
                    case Rank.Ace:
                        return 24;

                    case Rank.Ten:
                    case Rank.Jack:
                    case Rank.Queen:
                    case Rank.King:
                        return 25;

                    case Rank.Nine:
                        return 26;

                    case Rank.Eight:
                        return 27;

                    case Rank.Seven:
                        return 28;

                    case Rank.Six:
                        return 29;

                    case Rank.Five:
                        return 30;

                    case Rank.Four:
                        return 31;

                    case Rank.Three:
                        return 32;

                    case Rank.Two:
                        return 33;

                    default:
                        throw new InvalidOperationException();
                }
            }
            else if (playerHand.HasSoftAce)
            {
                switch (playerHand.SoftHighValue)
                {
                    case 20:
                        return 16;

                    case 19:
                        return 17;

                    case 18:
                        return 18;

                    case 17:
                        return 19;

                    case 16:
                        return 20;

                    case 15:
                        return 21;

                    case 14:
                        return 22;

                    case 13:
                        return 23;

                    default:
                        throw new InvalidOperationException();
                }
            }
            else
            {
                switch (playerHand.FinalValue)
                {
                    case 20:
                        return 0;

                    case 19:
                        return 1;

                    case 18:
                        return 2;

                    case 17:
                        return 3;

                    case 16:
                        return 4;

                    case 15:
                        return 5;

                    case 14:
                        return 6;

                    case 13:
                        return 7;

                    case 12:
                        return 8;

                    case 11:
                        return 9;

                    case 10:
                        return 10;

                    case 9:
                        return 11;

                    case 8:
                        return 12;

                    case 7:
                        return 13;

                    case 6:
                        return 14;

                    case 5:
                        return 15;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private static int GetActionColumn(Hand dealerHand)
        {
            switch (dealerHand.Cards[0].Rank)
            {
                case Rank.Two:
                    return 0;

                case Rank.Three:
                    return 1;

                case Rank.Four:
                    return 2;

                case Rank.Five:
                    return 3;

                case Rank.Six:
                    return 4;

                case Rank.Seven:
                    return 5;

                case Rank.Eight:
                    return 6;

                case Rank.Nine:
                    return 7;

                case Rank.Ten:
                case Rank.Jack:
                case Rank.Queen:
                case Rank.King:
                    return 8;

                case Rank.Ace:
                    return 9;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
