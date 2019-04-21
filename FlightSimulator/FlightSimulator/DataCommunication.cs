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
        public static void Send_string(string s)
        {
            string ip = ConfigurationManager.AppSettings["FlightServerIP"];
            short port = short.Parse(ConfigurationManager.AppSettings["FlightInfoPort"]);
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

            byte[] bytes = Encoding.ASCII.GetBytes(s);
            sock.Send(bytes);

            sock.Close();
        }
    }
}
