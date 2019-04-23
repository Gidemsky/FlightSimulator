using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using FlightSimulator.Views;
using FlightSimulator.ViewModels;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //DataBinding db;
        public MainWindow()
        {

            InitializeComponent();
            //db = new DataBinding();

            //DataContext = db;
            //MyJoystick.Moved += MyJoystick_Moved;
        }
    }
}
