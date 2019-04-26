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
         * Getter of 'isConnected'
         */
        internal bool GetIsConnected()
        {
            return(this.isConnceted);
        }

        /**
         * Setter of 'isConnected'
         */
        public void SetIsConnected(bool status)
        {
            this.isConnceted = status;
        }

        /**
         * Creates the Tcp Client connection to the server in order to send commands.
         */
        public void CreateCmdConnection(string ip, int port)
        {
            try
            {
                Console.WriteLine("Connecting to server. ip:{0}, on port:{1}", ip, port.ToString());
                client = new TcpClient(ip, port);
                stream = client.GetStream();
                stream.Flush();
                isConnceted = true;
                Console.WriteLine("Server Connected!");
            } catch (Exception exception)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(exception.Message);
            }
        }
            
        public void SendString(string path, double value)
        {
            string viewCommand = "set {0} {1}\r\n";
            viewCommand = String.Format(viewCommand, path, value);
            SendReadyMessage(viewCommand);
        }

        /**
         * Converts the string enterd to an array of strings
         * and sends the ready message to the relevant path in the simulator
         */
        public void SendReadyMessage(string command)
        {
            string[] cmd = CommandAdjustment(command);
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
                    // sends the commands to server
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
         * Adjust the command line to the relevant suitable data structure.
         */
        private string[] CommandAdjustment(string line)
        {
            string[] endLine = { "\r\n" };
            string[] readyOutPut = line.Split(endLine, StringSplitOptions.None);
            return readyOutPut;
        }
    }
}
