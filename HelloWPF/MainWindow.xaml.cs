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
        double x = 0, y = 0, z = 0;
        int d = 0, points = 0;
        int amountButtons = 20;
        public int tal = 30;
        int difficulty = 1;
        Random random = new Random();
        private List<Grid> DynamicGrids = new List<Grid>();
        DispatcherTimer _timer;
        TimeSpan _time;
        bool valid = true;
        ColumnDefinitionCollection columns;
        RowDefinitionCollection rows;
        bool mode;

        public MainWindow()
        {
            InitializeComponent();

            columns = gridmap.ColumnDefinitions;
            rows = gridmap.RowDefinitions;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            //var relative = e.GetPosition(this);
            //var point = PointToScreen(relative);

            //x = point.X;
            //y = point.Y;

            x++;
            z++;
            y++;

            //txt.Text = Convert.ToString(x);
            txt.Foreground = new SolidColorBrush(Colors.Black);

            
            if (x > 200)
            {
                //panel.Background = new SolidColorBrush(Color.FromRgb((byte)(x*random.NextDouble()), (byte)(z*random.NextDouble()), (byte)(y*random.NextDouble())));
                //panel.Background = new SolidColorBrush(Color.FromRgb((byte)x,(byte)z,(byte)y));
            }
            else
            {
            }

            txt.FontSize = 20;

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

            //window.Background = new SolidColorBrush(Color.FromRgb((byte)(x*random.NextDouble()), (byte)(z*random.NextDouble()), (byte)(y*random.NextDouble())));

            //_time = TimeSpan.Zero;
        }

        private void spawn_Button_Click(object sender, RoutedEventArgs e)
        {

            if (valid == true)
            {
                window.Background = new SolidColorBrush(Colors.White);
                points = 0;
                valid = false;
                _time = TimeSpan.FromSeconds(tal);

                gameTitle.Foreground = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
                gameTitle.FontWeight = FontWeights.Bold;
                gameTitle.Text = "GO GO GO!!!!";
                gameTitle.FontSize = 25;

                gridmap.MaxHeight = window.ActualHeight * 0.70;
                gridmap.MaxWidth = window.ActualWidth * 0.85;
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

                Thread.Sleep(900);

                gridmap.Children.Clear();

                for (int i = 0; i < amountButtons; i++)
                {
                    _timer.Start();
                    int randompositionY = random.Next(0, 11);
                    int randompositionX = random.Next(0, 11);
                    AddButton("box", "", randompositionX, randompositionY, gridmap, rowstr, columstr);
                }

            }
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
                //pointBlock.Text = points.ToString();
                }
                Grid.SetColumn(ch, (int)(random.Next(0, 11)));
                Grid.SetRow(ch, (int)(random.Next(0, 11)));
                ch.Background = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
                gameTitle.Foreground = new SolidColorBrush(Color.FromRgb((byte)(random.Next(256)), (byte)(random.Next(256)), (byte)(random.Next(256))));
            }

        }

        private void UpdateStuff()
        {
            Thread.Sleep(200);

            window.Background = new SolidColorBrush(Colors.White);
            gameTitle.Foreground = new SolidColorBrush(Colors.Black);
            gameTitle.FontWeight = FontWeights.Light;
            gameTitle.FontSize = 20;
            gameTitle.Text = "CLICK THE SQUARES!";

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



        //private void panel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("hehe");
        //}
    }
}
