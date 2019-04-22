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

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DataBinding db;
        DataCommunication my_comm;
        public MainWindow()
        {
            InitializeComponent();
            my_comm = new DataCommunication();
            db = new DataBinding(my_comm);
            DataContext = db;
            MyJoystick.Moved += MyJoystick_Moved;
            MyJoystick.Released += MyJoystick_Released;

            
            Thread thread = new Thread(new ParameterizedThreadStart(this.MyFunc));
            Thread thread2 = new Thread(new ParameterizedThreadStart(this.MyFunc));
            
            thread.Start(5);
            thread2.Start(199);
        }
        private void MyFunc(object nob)
        {
            int n = (int) nob;
            for (int i = 0; i < 100; ++i)
            {
                n += 1;
                Thread.Sleep(350 + new Random().Next(0, 350));

                text_box.Dispatcher.Invoke(
                    DispatcherPriority.Background,
                    new Action(() => text_box.Text = n.ToString()));
                
            }
        }

        private void MyJoystick_Released(Joystick sender)
        {
            db.Elevator = 0.0;
            db.Aileron = 0.0;
            my_comm.Send_string("\\controller\\aileron", db.Aileron);
        }

        private void MyJoystick_Moved(Joystick sender, Model.EventArgs.VirtualJoystickEventArgs args)
        {
            db.Elevator = args.Elevator;
            db.Aileron = args.Aileron;
            my_comm.Send_string("\\controller\\aileron", db.Aileron);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            text_box.Background = Brushes.PaleVioletRed;

        }

        private void Button_Clear(object sender, RoutedEventArgs e)
        {

            text_box.Background = Brushes.White;

        }

        private void MyJoystick_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
