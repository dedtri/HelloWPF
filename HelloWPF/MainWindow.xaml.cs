using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
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

            txt.Text = Convert.ToString(x);
            txt.Foreground = new SolidColorBrush(Colors.Black);

            
            if (x > 200)
            {
                //panel.Background = new SolidColorBrush(Color.FromRgb((byte)(x*random.NextDouble()), (byte)(z*random.NextDouble()), (byte)(y*random.NextDouble())));
                //panel.Background = new SolidColorBrush(Color.FromRgb((byte)x,(byte)z,(byte)y));
            }
            else
            {
            }

            txt.FontSize = x / 20;
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int randomposition = random.Next(300);
            x = 0;
            panel.Background = new SolidColorBrush(Color.FromRgb((byte)(x*random.NextDouble()), (byte)(z*random.NextDouble()), (byte)(y*random.NextDouble())));
        }

        private void spawn_Button_Click(object sender, RoutedEventArgs e)
        {
            int randompositionX = random.Next(5);
            int randompositionY = random.Next(5);

            panel.Background = new SolidColorBrush(Colors.Red);
        
            AddButton("XD", randompositionX, randompositionY, gridmap);
        }

        Button AddButton(string caption, int row, int column, Grid parent)
        {
            Button button = new Button();
            button.Content = caption;
            parent.Children.Add(button);
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);
            return button;
        }


        //private void panel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    MessageBox.Show("hehe");
        //}
    }
}
