using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator
{
    class FlightBoardBinding : BaseNotify, INotifyPropertyChanged
    {
        private ICommand settingButton;

        private ICommand connect;

        //private ICommand _disConnectCommand;

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

        private void OnClickConnect()
        {
            ServerConnect.Instance.createConnection(ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightInfoPort);
            //Commands.Instance.Open(ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightCommandPort);
            //if (!Commands.Instance.getIsConnect())
            //{
            //    new Thread(() =>
            //    {

            //        Info.Instance.connect();
            //        Commands.Instance.connect();

            //    }).Start();
            //}
            //else
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
                return settingButton ?? (settingButton = new CommandHandler(() => OnClick()));
            }
        }

        /**
         * Opens the settings window.
         * */
        private void OnClick()
        {
            var settingButton = new SettingButton();
            //settingButton.Show();
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
