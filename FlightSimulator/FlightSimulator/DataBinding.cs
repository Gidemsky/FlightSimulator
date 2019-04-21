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

        public DataBinding()
        {
            elevator = 0.0;
            aileron = 0.0;
        }
    }
}
