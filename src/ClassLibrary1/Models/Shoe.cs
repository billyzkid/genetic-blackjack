using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Models
{
    public sealed class Shoe
    {
        private const double MIN_CUT_PERCENTAGE = 0.2;
        private const double MAX_CUT_PERCENTAGE = 0.3;

        private Random random = new Random();
        private List<Card> cards;
        private int topCard = -1;
        private int cutCard = -1;

        public Shoe(int numDecks)
        {
            cards = Enumerable.Range(1, numDecks).Select(i => new Deck()).SelectMany(d => d.Cards).ToList();
        }

        public bool NeedsShuffle => topCard == -1 || cards.Count - topCard < cutCard;

        public Card NextCard()
        {
            return cards[topCard++];
        }

        public void BurnCard()
        {
            topCard++;
        }

        public void CutCards()
        {
            var cutPercentage = MIN_CUT_PERCENTAGE + (MAX_CUT_PERCENTAGE - MIN_CUT_PERCENTAGE) * random.NextDouble();
            cutCard = (int)(cards.Count * cutPercentage);
        }

        public void Shuffle()
        {
            for (var i = cards.Count - 1; i > 1; i--)
            {
                var card = cards[i];
                var r = random.Next(i);

                cards[i] = cards[r];
                cards[r] = card;
            }

            topCard = 0;
        }
    }
}
