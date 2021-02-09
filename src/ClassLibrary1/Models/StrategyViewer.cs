using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ClassLibrary1.Models
{
    public static class StrategyViewer
    {
        public static void Draw(Canvas canvas, IStrategy strategy, string caption, string savedImageName = null)
        {
            canvas.Children.Clear();

            AddBox(canvas, Colors.White, string.Empty, 0, 0);

            var x = 1;
            var y = 0;

            foreach (var dealerUpCardRank in new[] { Rank.Two, Rank.Three, Rank.Four, Rank.Five, Rank.Six, Rank.Seven, Rank.Eight, Rank.Nine, Rank.Ten, Rank.Ace })
            {
                var dealerHand = new Hand();
                dealerHand.Cards.Add(new Card(dealerUpCardRank, Suit.Clubs));

                AddBox(canvas, Colors.White, dealerUpCardRank.Description(), x, 0);

                y = 1;

                for (var hardTotal = 20; hardTotal >= 5; hardTotal--)
                {
                    AddBox(canvas, Colors.White, hardTotal.ToString(), 0, y);

                    var playerHand = new Hand();

                    switch (hardTotal)
                    {
                        case 20:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Eight, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 19:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Nine, Suit.Clubs));
                            break;

                        case 18:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Eight, Suit.Clubs));
                            break;

                        case 17:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Seven, Suit.Clubs));
                            break;

                        case 16:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Six, Suit.Clubs));
                            break;

                        case 15:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Five, Suit.Clubs));
                            break;

                        case 14:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Four, Suit.Clubs));
                            break;

                        case 13:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Three, Suit.Clubs));
                            break;

                        case 12:
                            playerHand.Cards.Add(new Card(Rank.Ten, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 11:
                            playerHand.Cards.Add(new Card(Rank.Nine, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 10:
                            playerHand.Cards.Add(new Card(Rank.Eight, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 9:
                            playerHand.Cards.Add(new Card(Rank.Seven, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 8:
                            playerHand.Cards.Add(new Card(Rank.Six, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 7:
                            playerHand.Cards.Add(new Card(Rank.Five, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 6:
                            playerHand.Cards.Add(new Card(Rank.Four, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;

                        case 5:
                            playerHand.Cards.Add(new Card(Rank.Three, Suit.Clubs));
                            playerHand.Cards.Add(new Card(Rank.Two, Suit.Clubs));
                            break;
                    }

                    var action = strategy.GetAction(playerHand, dealerHand);
                    AddBox(canvas, action.Color(), action.Description(), x, y);

                    y++;
                }

                foreach (var softRank in new[] { Rank.Nine, Rank.Eight, Rank.Seven, Rank.Six, Rank.Five, Rank.Four, Rank.Three, Rank.Two })
                {
                    AddBox(canvas, Colors.White, Rank.Ace.Description() + "-" + softRank.Description(), 0, y);

                    var playerHand = new Hand();
                    playerHand.Cards.Add(new Card(Rank.Ace, Suit.Clubs));
                    playerHand.Cards.Add(new Card(softRank, Suit.Clubs));

                    var action = strategy.GetAction(playerHand, dealerHand);
                    AddBox(canvas, action.Color(), action.Description(), x, y);

                    y++;
                }

                foreach (var pairRank in new[] { Rank.Ace, Rank.Ten, Rank.Nine, Rank.Eight, Rank.Seven, Rank.Six, Rank.Five, Rank.Four, Rank.Three, Rank.Two })
                {
                    AddBox(canvas, Colors.White, pairRank.Description() + "-" + pairRank.Description(), 0, y);

                    var playerHand = new Hand();
                    playerHand.Cards.Add(new Card(pairRank, Suit.Clubs));
                    playerHand.Cards.Add(new Card(pairRank, Suit.Diamonds));

                    var action = strategy.GetAction(playerHand, dealerHand);
                    AddBox(canvas, action.Color(), action.Description(), x, y);

                    y++;
                }

                x++;
            }

            AddCaption(canvas, caption, 0, y);

            if (!string.IsNullOrEmpty(savedImageName))
            {
                SaveCanvasToPng(canvas, savedImageName);
            }
        }

        private static void AddBox(Canvas canvas, Color color, string label, int x, int y)
        {
            var columnWidth = (int)canvas.ActualWidth / 13;
            var rowHeight = columnWidth * 4 / 7;
            var startX = columnWidth;
            var startY = columnWidth;

            var border = new Border();
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Background = new SolidColorBrush(color);
            border.Width = columnWidth;
            border.Height = rowHeight;
            canvas.Children.Add(border);

            var textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Text = label;
            textBlock.FontSize = 11;
            border.Child = textBlock;

            Canvas.SetLeft(border, startX + x * columnWidth);
            Canvas.SetTop(border, startY + y * rowHeight);
        }

        private static void AddCaption(Canvas canvas, string caption, int x, int y)
        {
            var columnWidth = (int)canvas.ActualWidth / 13;
            var rowHeight = columnWidth * 4 / 7;
            var startX = columnWidth;
            var startY = columnWidth;

            var textBlock = new TextBlock();
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Inlines.Add(new Bold(new Run(caption)));
            textBlock.FontSize = 16;
            textBlock.Width = columnWidth * 11;
            textBlock.TextAlignment = TextAlignment.Center;
            canvas.Children.Add(textBlock);

            Canvas.SetLeft(textBlock, startX + x * columnWidth);
            Canvas.SetTop(textBlock, startY + y * rowHeight + 10);
        }

        private static void SaveCanvasToPng(Canvas canvas, string imageName)
        {
            var size = canvas.RenderSize;
            var rect = new Rect(size);
            var rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96d, 96d, PixelFormats.Default);

            canvas.Measure(size);
            canvas.Arrange(rect);

            rtb.Render(canvas);

            var pngBitmapEncoder = new PngBitmapEncoder();
            pngBitmapEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var stream = new FileStream(imageName + ".png", FileMode.CreateNew))
            {
                pngBitmapEncoder.Save(stream);
            }
        }
    }
}
