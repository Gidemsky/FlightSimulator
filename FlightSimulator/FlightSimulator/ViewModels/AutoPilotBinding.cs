using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class AutoPilotBinding : BaseNotify
    {
        private string cmdAPLine;
        private ICommand okCommand = null;
        private ICommand clearCommand = null;
        CmdConnection connection;

        public AutoPilotBinding(CmdConnection cmdConnection)
        {
            cmdAPLine = "";
            connection = cmdConnection;
        }

        public ICommand OkButton
        {
            get
            {
                return okCommand ?? (okCommand = new CommandHandler(() => OnOkClick()));
            }
        }

        public ICommand ClearButton
        {
            get
            {
                return clearCommand ?? (clearCommand = new CommandHandler(() => OnClearClick()));
            }
        }

        private void OnOkClick()
        {
            connection.SendReadyMessage(cmdAPLine);
            OnClearClick();
        }

        public string TextProperty
        {
            set
            {
                cmdAPLine = value;
                NotifyPropertyChanged("TextProperty");
                NotifyPropertyChanged("Background");
            }
            get
            {
                return cmdAPLine;
            }
        }

        public string Background
        {
            get
            {
                return cmdAPLine == "" ? "White" : "Pink";
            }
        }

        private void OnClearClick()
        {
            cmdAPLine = "";
            NotifyPropertyChanged(cmdAPLine);
        }
    }
}
