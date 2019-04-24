using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {

        double lat;
        double lon;

        private static FlightBoardViewModel instanceFlightBoardViewModel = null;

        public FlightBoardViewModel()
        {
            instanceFlightBoardViewModel = this;
        }

        public static FlightBoardViewModel Instance
        {
            get
            {
                if (instanceFlightBoardViewModel == null)
                {
                    instanceFlightBoardViewModel = new FlightBoardViewModel();
                }
                return instanceFlightBoardViewModel;
            }
        }

        public double Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
                OnPropertyChanged("Lat");
            }
        }

        public double Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
                OnPropertyChanged("lon");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
