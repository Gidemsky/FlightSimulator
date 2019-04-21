using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace FlightSimulator
{
    class DataCommunication
    {
        readonly Socket sock;
        public DataCommunication()
        {
            string ip = FlightSimulator.Properties.Settings.Default.FlightServerIP;
            int port = FlightSimulator.Properties.Settings.Default.FlightCommandPort;
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(new IPEndPoint(IPAddress.Parse(ip), port));


        }

        public void Send_string(string path, double value)
        {
            string formatter = "set {0}={1}\r\n";
            string message = String.Format(formatter, path, value);

            byte[] bytes = Encoding.ASCII.GetBytes(message);
            sock.Send(bytes);
        }

        public void close()
        {
            sock.Close();
        }
    }
}
