using FlightSimulator.Model;
using FlightSimulator.Properties;
using FlightSimulator.ViewModels;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator
{
    class FlightBoardBinding : BaseNotify, INotifyPropertyChanged
    {
        private ICommand settings;

        private ICommand connect;

        double latitude;
        double longitude;

        #region

        public ICommand ConnectButton
        {
            get
            {
                return connect ?? (connect = new CommandHandler(() => OnClickConnect()));
            }
        }

        /**
         * the action that are taken when the "Connect" button us clicked
         */
        private void OnClickConnect()
        {
            if (!CmdConnection.Instance.getisConnected())
            {
                //create new two connections in seperated thread in order to avoid the program to stuck
                new Thread(() =>
                {

                    InfoConnection.Instance.infoCreateConnection(
                        ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightInfoPort);
                    CmdConnection.Instance.createCmdConnection(
                        ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightCommandPort);

                }).Start();
            }//TODO: check 
            //if - else
            //{
            //    new Thread(() =>
            //    {
            //        Commands.Instance.disConnect();
            //        Commands.Instance.connect();
            //    }).Start();
            //}
        }

        public ICommand SettingsCommand
        {
            get
            {
                return settings ?? (settings = new CommandHandler(() => OnClick()));
            }
        }

        /**
         * Opens the settings window.
         * */
        private void OnClick()
        {
            var settingButton = new Settings();
            //settingButton.show();
        }

        //public ICommand DisConnectCommand
        //{
        //    get
        //    {
        //        return _disConnectCommand ?? (_disConnectCommand = new CommandHandler(() => OnClickConnect()));
        //    }
        //}

        //private void OnClickDisConnect()
        //{
        //    Info.Instance.shouldStop = true;
        //    Commands.Instance.disConnect();
        //}
        #endregion

        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                OnPropertyChanged("LatitudeString");
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                OnPropertyChanged("LongitudeString");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
