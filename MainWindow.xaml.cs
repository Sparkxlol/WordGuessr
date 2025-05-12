 using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WordGuessr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Template Wordle
        Game game;
        StackPanel[] panels;
        private int round = 0;

        public MainWindow(DifficultyType difficulty)
        {
            InitializeComponent();

            game = new DefaultGame(difficulty);
            Debug.WriteLine(game.ActiveWord.ChosenWord);

            panels = [Layer0, Layer1, Layer2, Layer3, Layer4, Layer5];

            for (int i = 1; i < panels.Length; i++)
            {
                foreach (TextBox child in panels[i].Children)
                {
                    child.IsReadOnly = true;
                }
            }

            Layer0.Children[0].Focus();
        }

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TextBox child in panels[round].Children)
            {
                sb.Append(child.Text);
            }

            string word = sb.ToString();
            LetterType[]? comparison = game.PlayRound(word);

            if (comparison == null)
            {
                return;
            }

            int letterIndex = 0;
            foreach (TextBox child in panels[round].Children)
            {
                // Set the TextBox to read-only immediately after the round
                child.IsReadOnly = true;

                // Define the background color based on the comparison result
                Color targetColor = comparison[letterIndex] switch
                {
                    LetterType.Green => Colors.Green,
                    LetterType.Yellow => Colors.Yellow,
                    LetterType.Gray => Colors.Gray,
                    _ => Colors.White // Default color
                };

                // Create a RotateTransform for 2D rotation (Z-axis)
                var rotateTransform = new RotateTransform
                {
                    Angle = 0, // Start at 0 degrees
                    CenterX = child.ActualWidth / 2,
                    CenterY = child.ActualHeight / 2
                };

                // Apply the rotation transform to the TextBox
                child.RenderTransform = rotateTransform;

                // Create the animation to rotate the TextBox by 360 degrees
                var rotateAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 360,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)), // Duration of rotation
                    AutoReverse = false
                };

                // Begin the animation for the rotation
                rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

                // Create the color animation to gradually change the background color
                var colorAnimation = new ColorAnimation
                {
                    From = ((SolidColorBrush)child.Background).Color,
                    To = targetColor,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)), // Duration of color change
                    AutoReverse = false
                };

                // Begin the color animation
                var colorBrush = new SolidColorBrush(((SolidColorBrush)child.Background).Color);
                child.Background = colorBrush; // Set initial background color
                colorBrush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);

                // Add a small delay to update each box one after another
                await Task.Delay(500); // Adjust this delay if needed

                letterIndex++;
            }
            if (round != 5 && game.Complete == false)
            {
                foreach (TextBox child in panels[round + 1].Children)
                {
                    child.IsReadOnly = false;
                }

                panels[round + 1].Children[0].Focus();
            }

            round++;
            if (game.Victory)
            {
                ShowConfetti();
                PlaySound("success.mp3");
            }
            else if(!game.Victory && round == 6)
            {
                PlaySound("loss.mp3");
            }
        }
         
        private void LetterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            TextBox textBox = sender as TextBox;
            textBox.Text = textBox.Text.ToUpper();
            StackPanel currentPanel = panels[round];

            for (int i = 0; i < currentPanel.Children.Count - 1; i++)
            {
                if (sender == currentPanel.Children[i])
                {
                    currentPanel.Children[i + 1].Focus();
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GoButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void LetterTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                StackPanel currentPanel = panels[round];

                for (int i = 1; i < currentPanel.Children.Count; i++)
                {
                    if (sender == currentPanel.Children[i])
                    {
                        TextBox child = (TextBox) currentPanel.Children[i];

                        if (child.Text.Length > 0)
                        {
                            ((TextBox)currentPanel.Children[i]).Text = "";
                        }
                        else
                        {
                            ((TextBox)currentPanel.Children[i - 1]).Text = "";
                            currentPanel.Children[i - 1].Focus();
                        }
                    }
                }
            }
        }
        private MediaPlayer? player;
        private void PlaySound(string filename)
        {
            player = new MediaPlayer();
            player.Open(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", filename)));
            player.Play();
        }

        private void ShowConfetti()
        {
            Random rand = new Random();

            for (int i = 0; i < 300; i++)
            {
                var rect = new Rectangle
                {
                    Width = 5,
                    Height = 10,
                    Fill = new SolidColorBrush(Color.FromRgb(
                        (byte)rand.Next(256),
                        (byte)rand.Next(256),
                        (byte)rand.Next(256)))
                };

                Canvas.SetLeft(rect, rand.Next((int)this.ActualWidth));
                Canvas.SetTop(rect, -10);
                ConfettiCanvas.Children.Add(rect);

                DoubleAnimation fall = new DoubleAnimation
                {
                    From = -10,
                    To = this.ActualHeight + 20,
                    Duration = TimeSpan.FromSeconds(rand.NextDouble() * 2 + 1),
                    FillBehavior = FillBehavior.Stop
                };

                fall.Completed += (s, e) => ConfettiCanvas.Children.Remove(rect);

                rect.BeginAnimation(Canvas.TopProperty, fall);
            }
        }

    }
}