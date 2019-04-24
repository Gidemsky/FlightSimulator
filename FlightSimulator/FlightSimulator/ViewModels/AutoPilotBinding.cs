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
        private ICommand _okCommand;
        private ICommand _clearCommand;
        private string text;
        private CmdConnection model;

        public AutoPilotBinding(CmdConnection mod)
        {
            text = "";
            model = mod;
        }

        public string AutoPilotCommands
        {
            set
            {
                text = value;
                NotifyPropertyChanged("AutoPilotCommands");
                NotifyPropertyChanged("Color");
            }

            get
            {
                return text;
            }
        }

        public string Color
        {
            get
            {
                if (text == "")
                {
                    return "White";
                }
                else
                {
                    return "Pink";
                }
            }
        }

        private void Parser()
        {
            string[] delimiter = { "\r\n" };
            List<string> result = text.Split(delimiter, StringSplitOptions.None).ToList();
            text = "";
            NotifyPropertyChanged("Color");
            //model.sendMessage(result);
        }

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ?? (_okCommand = new CommandHandler(() => OkClick()));
            }
        }

        private void OkClick()
        {

        }

        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ?? (_clearCommand = new CommandHandler(() => ClearClick()));
            }
        }

        private void ClearClick()
        {
            text = "";
            NotifyPropertyChanged(text);

        }
    }
}
