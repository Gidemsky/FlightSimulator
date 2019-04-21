using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator
{
    class DataBinding: INotifyPropertyChanged
    {
        double elevator;
        double aileron;
        double rudder;
        double throttle;

        double prev_rudder;
        double prev_throttle;

        const double MIN_RUDDER_CHANGE = 0.01;
        const double MIN_THROTTLE_CHANGE = 0.01;



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public double Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                elevator = value;
                OnPropertyChanged("ElevatorString");
            }
        }
        
        public double Aileron
        {
            get
            {
                return aileron;
            }
            set
            {
                aileron = value;
                OnPropertyChanged("AileronString");
            }
        }

        public double Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                if (Math.Abs(rudder - prev_rudder) < MIN_RUDDER_CHANGE)
                {
                    rudder = value;
                    return;
                }
                rudder = prev_rudder = value;
                data_comm.Send_string("\\controls\\rudder", rudder);
                OnPropertyChanged("RudderString");
                OnPropertyChanged("Rudder");
            }
        }

        public double Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
                OnPropertyChanged("ThrottleString");
                OnPropertyChanged("Throttle");
            }
        }

        public string ElevatorString
        {
            get
            {
                return elevator.ToString();
            }
        }
        
        public string AileronString
        {
            get
            {
                return aileron.ToString();
            }
        }

        public string RudderString
        {
            get
            {
                double intermediate = Math.Truncate(rudder * 100) / 100;
                return String.Format("{0:N2}", intermediate);
            }
        }

        public string ThrottleString
        {
            get
            {
                double intermediate = Math.Truncate(throttle * 100) / 100;
                return String.Format("{0:N2}", intermediate);
            }
        }

        public static double MIN_THROTTLE_CHANGE1 => MIN_THROTTLE_CHANGE;

        DataCommunication data_comm;
        public DataBinding(DataCommunication data_comm)
        {
            this.data_comm = data_comm;
        }

    }
}
