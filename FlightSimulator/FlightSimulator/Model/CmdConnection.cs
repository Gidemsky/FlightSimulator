using FlightSimulator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    /**
     * The command communication socket.
     * this is client user of the simulator that responsiable sending commands to the simulator.
     */
    class CmdConnection : BaseNotify
    {
        private static CmdConnection self;
        bool isConnceted;
        TcpClient client;
        Thread sendingThread;
        private NetworkStream stream = null;

        public static CmdConnection Instance
        {
            get
            {
                if (self == null)
                {
                    self = new CmdConnection();
                }
                return self;
            }
        }

        /**
         * getter of the member isconnected the acts as tag of the correct situation
         */
        internal bool getisConnected()
        {
            return(this.isConnceted);
        }

        /**
         * Creates the Tcp Client connection to the server in order to send commands.
         */
        public void createCmdConnection(string ip, int port)
        {
            try
            {
                Console.WriteLine("Trying to connecting");
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                Console.WriteLine("connected!!!");
                stream.Flush();
                isConnceted = true;
                Console.WriteLine("connected to the ip:{0} on the port:{1}", ip, port.ToString());
            } catch (Exception exception)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(exception.Message);
            }
        }
            
        public void Send_string(string path, double value)
        {
            string viewCommand = "set {0} {1}\r\n";
            viewCommand = String.Format(viewCommand, path, value);
            Console.WriteLine("The out going command is:{0}",viewCommand);
            SendReadyMessage(viewCommand);
        }

        /**
         * converts the string enterd to an array of strings
         * and sends the ready message to the relevant path in the simulator
         */
        public void SendReadyMessage(string command)
        {
            string[] cmd = commandAdjustment(command);
            sendingThread = new Thread(() =>
            {
                if (!isConnceted)
                {
                    return;
                }
                string specificCommandLine = null;
                NetworkStream ns = client.GetStream();
                foreach (string split in cmd)
                {
                    // Sends the commands to server
                    specificCommandLine = split;
                    specificCommandLine += "\r\n";
                    byte[] buffer = Encoding.ASCII.GetBytes(specificCommandLine);
                    ns.Write(buffer, 0, buffer.Length);
                    Thread.Sleep(1500);
                }
            });
            sendingThread.Start();
        }

        /**
         * adjust the command line to the right suitable data structure
         */
        private string[] commandAdjustment(string line)
        {
            string[] endLine = { "\r\n" };
            string[] readyOutPut = line.Split(endLine, StringSplitOptions.None);
            return readyOutPut;
        }

        //public void Close()
        //{
        //    isConnceted = true;
        //}

        //public bool IsOpen()
        //{
        //    return this.tcpListener != null;
        //}

        //public string Data
        //{
        //    get
        //    {
        //        return socketData;
        //    }
        //    set
        //    {
        //        socketData = value;
        //        NotifyPropertyChanged("Data");
        //    }
        //}

    }
}
