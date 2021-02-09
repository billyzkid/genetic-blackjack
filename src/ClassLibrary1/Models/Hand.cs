using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Models
{
    public sealed class Hand
    {
        public const int MIN_VALUE = 0;
        public const int MAX_VALUE = 21;
        public const int DEALER_STAND_VALUE = 17;

        public Hand()
        {
            Cards = new List<Card>();
        }

        public IList<Card> Cards { get; }
        public IList<Card> HardCards => Cards.Where(c => c.Rank != Rank.Ace).ToList();
        public IList<Card> Aces => Cards.Where(c => c.Rank == Rank.Ace).ToList();

        public bool HasAces => Aces.Any();
        public bool HasSoftAce => HasAces && SoftHighValue <= MAX_VALUE;
        public bool HasPair => Cards.Count == 2 && Cards[0].Rank == Cards[1].Rank;

        public int HardValue => HardCards.Sum(c => c.Rank.LowValue());
        public int SoftLowValue => HasAces ? Rank.Ace.LowValue() * Aces.Count + HardValue : MIN_VALUE;
        public int SoftHighValue => HasAces ? Rank.Ace.HighValue() + Rank.Ace.LowValue() * (Aces.Count - 1) + HardValue : MIN_VALUE;
        public int FinalValueSlow => HasSoftAce ? SoftHighValue : HasAces ? SoftLowValue : HardValue;
        public int FinalValue
        {
            get
            {
                int highValue = 0;
                int lowValue = 0;
                bool aceWasUsedAsHigh = false;

                foreach (var card in Cards)
                {
                    if (card.Rank == Rank.Ace)
                    {
                        if (!aceWasUsedAsHigh)
                        {
                            aceWasUsedAsHigh = true;
                            highValue += card.Rank.HighValue();
                            lowValue += card.Rank.LowValue();
                        }
                        else
                        {
                            highValue += card.Rank.LowValue();
                            lowValue += card.Rank.LowValue();
                        }
                    }
                    else
                    {
                        highValue += card.Rank.LowValue();
                        lowValue += card.Rank.LowValue();
                    }
                }

                if (highValue <= 21)
                {
                    return highValue;
                }

                return lowValue;
            }
        }

        public override string ToString()
        {
            var s = string.Join(", ", Cards.Select(c => c.ToString()));

            if (HasSoftAce)
            {
                s += $" ({SoftLowValue} or {SoftHighValue})";
            }
            else
            {
                s += $" ({FinalValue})";
            }

            return s;
        }
    }
}
