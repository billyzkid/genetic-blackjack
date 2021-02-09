namespace ClassLibrary1.Models
{
    public sealed class Card
    {
        public Card(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public Rank Rank { get; }
        public Suit Suit { get; }

        public override string ToString()
        {
            return Rank.Description() + Suit.Description();
        }
    }
}
