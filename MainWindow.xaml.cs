using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

        public MainWindow()
        {
            InitializeComponent();

            game = new DefaultGame(DifficultyType.Easy);
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

        private void GoButton_Click(object sender, RoutedEventArgs e)
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
                child.IsReadOnly = true;
                
                switch (comparison[letterIndex])
                {
                    case LetterType.Green:
                        child.Background = new SolidColorBrush(Colors.Green);
                        break;
                    case LetterType.Yellow:
                        child.Background = new SolidColorBrush(Colors.Yellow);
                        break;
                    case LetterType.Gray:
                        child.Background = new SolidColorBrush(Colors.Gray);
                        break;
                }

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
        }

        private void LetterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
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
    }
}