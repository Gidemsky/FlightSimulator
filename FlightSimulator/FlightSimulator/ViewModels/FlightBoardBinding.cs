﻿using FlightSimulator.Model;
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
        private ICommand settingsCommand;
        private ICommand connect;
        Settings settings;
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
            if (!CmdConnection.Instance.GetIsConnected())
            {
                // create two connections in seperated thread in order to avoid the program to stuck
                new Thread(() =>
                {

                    InfoConnection.Instance.InfoCreateConnection(
                        ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightInfoPort);
                    CmdConnection.Instance.CreateCmdConnection(
                        ApplicationSettingsModel.Instance.FlightServerIP, ApplicationSettingsModel.Instance.FlightCommandPort);

                }).Start();
            }
        }
        
        public ICommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new CommandHandler(() => OnClick()));
            }
        }

        /**
         * Opens the settings window.
         * */
        private void OnClick()
        {
            settings = new Settings();
            settings.ShowDialog();
        }
        #endregion
    }
}
