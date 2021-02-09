using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary1.Models
{
    public static class StrategyTester
    {
        public static double Test(IStrategy strategy)
        {
            var numDecks = Settings.Current.TestSettings.NumDecks;
            var numRounds = Settings.Current.TestSettings.NumRounds;
            var betAmount = Settings.Current.TestSettings.BetAmount;
            var blackjackPayout = Settings.Current.TestSettings.BlackjackPayout;

            var shoe = new Shoe(numDecks);
            var dealerHand = new Hand();
            var playerHand = new Hand();
            var playerHands = new List<Hand>();
            var playerBets = new List<double>();
            var playerChips = 0d;

            for (var n = 0; n < numRounds; n++)
            {
                if (shoe.NeedsShuffle)
                {
                    shoe.Shuffle();
                    shoe.CutCards();
                    shoe.BurnCard();
                }

                playerHand.Cards.Clear();
                dealerHand.Cards.Clear();

                playerHand.Cards.Add(shoe.NextCard());
                dealerHand.Cards.Add(shoe.NextCard());
                playerHand.Cards.Add(shoe.NextCard());
                dealerHand.Cards.Add(shoe.NextCard());

                playerHands.Clear();
                playerHands.Add(playerHand);

                playerBets.Clear();
                playerBets.Add(betAmount);
                playerChips -= betAmount;

                // 1. if the player has blackjack
                if (playerHand.FinalValue == Hand.MAX_VALUE)
                {
                    // if the dealer also has blackjack
                    if (dealerHand.FinalValue == Hand.MAX_VALUE)
                    {
                        // it's a tie; return the bet
                        playerChips += playerBets[0];
                    }
                    else
                    {
                        // the player won; return the bet plus a matching amount multiplied by the blackjack payout
                        playerChips += playerBets[0] + playerBets[0] * blackjackPayout;
                    }

                    // move to the next hand
                    continue;
                }

                // 2. if the dealer has blackjack
                if (dealerHand.FinalValue == Hand.MAX_VALUE)
                {
                    // move to the next hand
                    continue;
                }

                // 3. play the player's hand plus any split hands
                for (var h = 0; h < playerHands.Count; h++)
                {
                    playerHand = playerHands[h];

                    var gameState = GameState.PlayerDrawing;

                    while (gameState == GameState.PlayerDrawing)
                    {
                        if (playerHand.FinalValue == Hand.MAX_VALUE)
                        {
                            // if a split hand has blackjack
                            if (playerHand.Cards.Count == 2)
                            {
                                // return the bet plus a matching amount multiplied by the blackjack payout
                                playerChips += playerBets[h] + playerBets[h] * blackjackPayout;
                                playerBets[h] = 0;
                            }

                            // automatically stand at 21
                            gameState = GameState.DealerDrawing;
                            break;
                        }

                        // stand, hit, double, or split
                        var action = strategy.GetAction(playerHand, dealerHand);

                        // if attempting to double with more than 2 cards, then hit instead
                        if (action == Action.Double && playerHand.Cards.Count > 2)
                        {
                            action = Action.Hit;
                        }

                        switch (action)
                        {
                            case Action.Stand:
                                gameState = GameState.DealerDrawing;
                                break;

                            case Action.Hit:
                                // deal the next card
                                playerHand.Cards.Add(shoe.NextCard());

                                // if the player busted
                                if (playerHand.FinalValue > Hand.MAX_VALUE)
                                {
                                    playerBets[h] = 0;
                                    gameState = GameState.PlayerBusted;
                                }
                                else if (playerHand.FinalValue == Hand.MAX_VALUE)
                                {
                                    // automatically stand at 21
                                    gameState = GameState.DealerDrawing;
                                }

                                break;

                            case Action.Double:
                                // double the bet
                                playerChips -= betAmount;
                                playerBets[h] += betAmount;

                                // deal one and only one card
                                playerHand.Cards.Add(shoe.NextCard());

                                // if the player busted
                                if (playerHand.FinalValue > Hand.MAX_VALUE)
                                {
                                    playerBets[h] = 0;
                                    gameState = GameState.PlayerBusted;
                                }
                                else
                                {
                                    gameState = GameState.DealerDrawing;
                                }

                                break;

                            case Action.Split:
                                // add the split hand
                                var splitHand = new Hand();
                                splitHand.Cards.Add(playerHand.Cards[1]);
                                playerHand.Cards[1] = shoe.NextCard();
                                splitHand.Cards.Add(shoe.NextCard());
                                playerHands.Add(splitHand);

                                // add the extra bet
                                playerChips -= betAmount;
                                playerBets.Add(betAmount);

                                break;
                        }
                    }
                }

                // 4. if the player has any "active" hands remaining (i.e. hands with a non-zero bet), then play the dealer hand
                if (playerBets.Sum() > 0)
                {
                    var gameState = GameState.DealerDrawing;

                    // the dealer must draw until 17 or busted
                    while (dealerHand.FinalValue < Hand.DEALER_STAND_VALUE)
                    {
                        // deal the next card
                        dealerHand.Cards.Add(shoe.NextCard());

                        // if the dealer busted
                        if (dealerHand.FinalValue > Hand.MAX_VALUE)
                        {
                            // payoff each active player hand
                            for (var h = 0; h < playerHands.Count; h++)
                            {
                                // return the bet plus a matching amount
                                playerChips += playerBets[h] * 2;
                            }

                            gameState = GameState.DealerBusted;
                            break;
                        }
                    }

                    // if the dealer has not busted
                    if (gameState != GameState.DealerBusted)
                    {
                        var dealerHandFinalValue = dealerHand.FinalValue;

                        // compare the dealer hand to each player hand
                        for (var h = 0; h < playerHands.Count; h++)
                        {
                            var playerHandFinalValue = playerHands[h].FinalValue;

                            if (playerHandFinalValue == dealerHandFinalValue)
                            {
                                // it's a tie; return the bet
                                playerChips += playerBets[h];
                            }
                            else if (playerHandFinalValue > dealerHandFinalValue)
                            {
                                // the player won; return the bet plus a matching amount
                                playerChips += playerBets[h] * 2;
                            }
                            else
                            {
                                // the player lost
                            }
                        }
                    }
                }
            }

            return playerChips;
        }

        public static void GetStatistics(IStrategy strategy, out double avg, out double stdDev, out double coeffVariation)
        {
            var numTests = Settings.Current.TestSettings.NumTests;
            var scores = new ConcurrentBag<double>();

            Parallel.For(0, numTests, i =>
            {
                var score = Test(strategy);
                scores.Add(score);
            });

            avg = scores.Average();
            stdDev = scores.StandardDeviation();
            coeffVariation = stdDev / avg;
        }
    }
}
