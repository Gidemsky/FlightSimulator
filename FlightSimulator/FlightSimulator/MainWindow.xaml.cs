using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using FlightSimulator.Views;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataBinding db;
        public MainWindow()
        {
            InitializeComponent();
            db = new DataBinding();
            DataContext = db;
            MyJoystick.Moved += MyJoystick_Moved;
        }

        private void MyJoystick_Moved(Joystick sender, Model.EventArgs.VirtualJoystickEventArgs args)
        {
            db.Elevator = args.Elevator;
            db.Aileron = args.Aileron;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            DataCommunication.Send_string("Hello, world!");

        }
    }
}
