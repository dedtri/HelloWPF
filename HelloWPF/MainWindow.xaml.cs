using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HelloWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Deklarationer

        int d = 0, points = 0;
        int amountButtons = 20;
        public int tal = 15;
        int difficulty = 1;
        Random random = new Random();
        DispatcherTimer _timer;
        TimeSpan _time;
        bool valid = true, mode;
        ColumnDefinitionCollection columns;
        RowDefinitionCollection rows;
       
        public MainWindow()
        {
            InitializeComponent();

            columns = gridmap.ColumnDefinitions;
            rows = gridmap.RowDefinitions;
        }

        //Min "win" checker, det er en MouseMove funktion, dvs at den tjekker hele tiden mens man bevæger musen.

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (points == amountButtons)
            {
                _time = TimeSpan.Zero;
                window.Background = new SolidColorBrush(Colors.Green);
                HideStuff();
                MoveTitle();
                gameTitle.Text = "YOU'RE INSANEEEEEE!!!!!";
                MessageBox.Show("CONGRAUTLATIONS!!!");
                window.Background = new SolidColorBrush(Colors.White);           
                points = 0;
                UpdateStuff();
            }
        }

        //ButtonClick function som aktiveres ved at trykke på knappen. Den styer om det skal være nemt eller svært.

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mode = !mode;

            if (mode)
            { 
            difficulty = 2;
            difficultyButton.Content = "CHALLENGE MODE";
            }
            else
            {
                difficulty = 1;
                difficultyButton.Content = "NORMAL MODE";
            }
        }

        //Endnu en ButtonClick function som aktiveres ved at trykke på knappen dog med andre funktioner.

        private void spawn_Button_Click(object sender, RoutedEventArgs e)
        {

            if (valid == true)
            {
                window.Background = new SolidColorBrush(Colors.White);
                points = 0;
                valid = false;
                _time = TimeSpan.FromSeconds(tal);

                //Når man skal ændre properties i C# "code behind" delen, skal man bruge nogle andre syntaxer og objekter, f.eks. SolidColorBrush
                //Her opdaterer jeg min game title efter "spillet" er gået i gang med en tilfældig RGB farve

                gameTitle.Foreground = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
                gameTitle.FontWeight = FontWeights.Bold;
                gameTitle.Text = "GO GO GO!!!!";
                gameTitle.FontSize = 25;


                //Her er "playing field" sat til at opdatere vinduet efter man trykker på "START" knappen, så størrelsen på "gridmap" er dynamisk

                gridmap.MaxHeight = window.ActualHeight * 0.70;
                gridmap.MaxWidth = window.ActualWidth * 0.85;

                //Her adjuster jeg mine individuelle kolonner og rækker til at justere sig til gridmap's størrelse

                int columstr = (int)gridmap.MaxWidth / gridmap.ColumnDefinitions.Count;
                int rowstr = (int)gridmap.MaxHeight / gridmap.RowDefinitions.Count;

                foreach (var col in columns)
                {
                    col.Width = new GridLength(columstr);
                }
                foreach (var row in rows)
                {
                    row.Height = new GridLength(rowstr);
                }

                //Her er min "timer" som begynder efter man trykker på knappen, det er funktionen der 
                //Den er også med til at meddele at man har tabt, hvis timeren rammer 0 før man har clearet alle buttons

                _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
                {
                    tbTime.Text = _time.ToString();
                    if (_time == TimeSpan.Zero)
                    {
                        _timer.Stop();
                        gridmap.Children.Clear();
                        valid = true;
                        if (points < amountButtons)
                        {
                            window.Background = new SolidColorBrush(Colors.Red);
                            HideStuff();
                            MoveTitle();
                            gameTitle.Text = "TOO BAD!";
                            MessageBox.Show("YOU LOSE!!!");
                            UpdateStuff();
                        }
                    }
                    _time = _time.Add(TimeSpan.FromSeconds(-1));
                    
                }, Application.Current.Dispatcher); _timer.Start();

                //Pågrund af hvordan TimeSpan og DispatcherTimer fungerer har jeg tilføjet en lille forsinkelse til ta få dem til at sync op med kreationen af knapperne

                Thread.Sleep(900);

                gridmap.Children.Clear();

                //Loop til sende parameter afsted til min AddButton funktion og køre den.

                for (int i = 0; i < amountButtons; i++)
                {
                    _timer.Start();
                    int randompositionY = random.Next(0, 11);
                    int randompositionX = random.Next(0, 11);
                    AddButton("box", "", randompositionX, randompositionY, gridmap, rowstr, columstr);
                }

            }

            //Hele knappen er i en if/else, hvis der stadig er knapper tilbage hvis man trykker på START, så får man en error besked

            else
            {
                window.Background = new SolidColorBrush(Colors.Red);
                MessageBox.Show("ERROR - YOU ALREADY HAVE BOXES TO CLICK!! TRY AGAIN");
                UpdateStuff();
                _time = TimeSpan.Zero;
                gridmap.Children.Clear();
                points = amountButtons+1;
                bool valid = true;
            }
        }


        //Funktion til at styre ens "timer", En Textbox man kan skrive i, og når man trykker enter sender den informationen afsted
        //Så bruger man SetTimer.Text til at justere tiden

        private void SetTimer_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                try
                {
                    if (Convert.ToInt32(SetTimer.Text) <= 30)
                    { 
                    tal = Convert.ToInt32(SetTimer.Text);
                        MessageBox.Show("Your time is now: " + SetTimer.Text + " " + "seconds.");
                        if (Convert.ToInt32(SetTimer.Text) >= 21)
                        {
                            SetTimerText.Text = "Adjust your timer! EASY (" + SetTimer.Text + "s)";
                            SetTimerText.Foreground = new SolidColorBrush(Colors.Green);
                        }
                        else if (Convert.ToInt32(SetTimer.Text) >= 11)
                        {
                            SetTimerText.Text = "Adjust your timer! MEDIUM (" + SetTimer.Text + "s)";
                            SetTimerText.Foreground = new SolidColorBrush(Colors.Blue);
                        }
                        else if (Convert.ToInt32(SetTimer.Text) >= 0)
                        {
                            SetTimerText.Text = "Adjust your timer! HARD (" + SetTimer.Text + "s)";
                            SetTimerText.Foreground = new SolidColorBrush(Colors.Red);
                        }


                    }
                    else
                    {
                        MessageBox.Show("MAX 30 SECONDS!");
                    }
                }
                  catch
                {
                    MessageBox.Show("Non actual number detected.");
                }
            }
        }

        //Min AddButton funktion, som creater de forskellige knapper der kommer ind på ens playing field når loopet i "START" knappen starter.

        public void AddButton(string name, string caption, int row, int column, Grid parent, int rowstr, int colstr)
        {

            Button button = new Button();

                d++;
                button.Name = name + d.ToString();
                button.Content = caption;
                button.Height = rowstr / difficulty;
                button.Width = colstr / difficulty;
                button.Background = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
                button.Click += new RoutedEventHandler(test_Read_Click);
                var testvalue = row + column;
                parent.Children.Add(button);

        }

        //Funktionen til at slette knapper man trykker på

        private void test_Read_Click(object sender, RoutedEventArgs e)
        {

            List<Button> remove = new List<Button>();
            foreach (var children in gridmap.Children)
            {
                if ((children.GetType() == typeof(Button)))
                {
                        remove.Add(children as Button);
                }
            }
            foreach (var ch in remove)
            {
                if ((ch as Button).Name == (sender as Button).Name)
                {
                gridmap.Children.Remove(ch as Button);
                points++;
                }
                Grid.SetColumn(ch, (int)(random.Next(0, 11)));
                Grid.SetRow(ch, (int)(random.Next(0, 11)));
                ch.Background = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
                gameTitle.Foreground = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
            }

        }

        //Update metode til at spare noget kode

        private void UpdateStuff()
        {
            Thread.Sleep(200);

            window.Background = new SolidColorBrush(Colors.White);
            gameTitle.Foreground = new SolidColorBrush(Colors.Black);
            gameTitle.FontWeight = FontWeights.Light;
            gameTitle.FontSize = 20;
            gameTitle.Text = "SQUARE ATTACK";

            Thickness m = gameTitle.Margin;
            m.Left = 90;
            m.Top = 20;
            m.Right = 20;
            m.Bottom = 20;
                                             
            gameTitle.Margin = m;

            difficultyButton.Visibility = Visibility.Visible;
            spawn.Visibility = Visibility.Visible;
            tbTime.Visibility = Visibility.Visible;
            SetTimer.Visibility = Visibility.Visible;
            SetTimerText.Visibility = Visibility.Visible;
        }

        //Funktionen HideStuff hjælper med at cleane mit MainWindow for kun at vise min "titel" som bliver opdateret dynamisk med indhold og position.

        private void HideStuff()
        {
            difficultyButton.Visibility = Visibility.Hidden;
            spawn.Visibility = Visibility.Hidden;
            tbTime.Visibility = Visibility.Hidden;
            SetTimer.Visibility = Visibility.Hidden;
            SetTimerText.Visibility = Visibility.Hidden;
        }

        private void MoveTitle()
        {
            Thickness m = gameTitle.Margin;
            m.Left = 90;
            m.Top = 300;
            m.Right = 20;
            m.Bottom = 20;

            gameTitle.Margin = m;
        }
    }
}
