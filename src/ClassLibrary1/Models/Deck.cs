using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models
{
    public sealed class Deck
    {
        public Deck()
        {
            Cards = new List<Card>(52);

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    Cards.Add(new Card(rank, suit));
                }
            }
        }

        public IList<Card> Cards { get; }
    }
}
