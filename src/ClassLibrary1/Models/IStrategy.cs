namespace ClassLibrary1.Models
{
    public interface IStrategy
    {
        Action GetAction(Hand playerHand, Hand dealerHand);
    }
}
