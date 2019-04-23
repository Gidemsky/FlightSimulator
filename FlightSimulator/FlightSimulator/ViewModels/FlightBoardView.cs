using FlightSimulator.Model;
using FlightSimulator.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class FlightBoardView : BaseNotify
    {
        private ICommand settingButton;

        private ICommand connect;

        //private ICommand _disConnectCommand;

        private double lat, lon;
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
    }
}
