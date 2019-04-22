using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator
{
    class InfoCommunication
    {
        private float lon, lat;

        Thread threadI;

        TcpClient _client;

        TcpListener listener;

        public bool shouldStop
        {
            get;
            set;
        }

        public float Lon
        {
            get
            {
                return lon;
            }
            set
            {
                lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        public float Lat
        {
            get
            {
                return lat;
            }
            set
            {
                lat = value;
                NotifyPropertyChanged("Lat");
            }
        }

        private static InfoCommunication m_Instance = null;

        public static InfoCommunication Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new InfoCommunication();
                }
                return m_Instance;
            }
        }

        private Info()
        {
            shouldStop = false;
        }

        public void closeThread()
        {
            threadI.Abort();
        }

        public void disConnect()
        {
            shouldStop = true;
            _client.Close();
        }

        public void connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ApplicationSettingsModel.Instance.FlightServerIP),
                ApplicationSettingsModel.Instance.FlightInfoPort);
            listener = new TcpListener(ep);
            listener.Start();
            //Console.WriteLine("Waiting for client connections...");
            _client = listener.AcceptTcpClient();
            Console.WriteLine("Info channel: Client connected");
            threadI = new Thread(() => listen(_client));
            threadI.Start();
        }

        public void listen(TcpClient _client)
        {
            Byte[] bytes;
            //string[] splitMsg = new string[23];
            //string lon, lat;
            NetworkStream ns = _client.GetStream();
            while (!shouldStop)
            {
                if (_client.ReceiveBufferSize > 0)
                {
                    bytes = new byte[_client.ReceiveBufferSize];
                    ns.Read(bytes, 0, _client.ReceiveBufferSize);
                    string msg = Encoding.ASCII.GetString(bytes); //the message incoming
                    splitMsg(msg);
                    Console.WriteLine("info");
                    Console.WriteLine(Lon);
                    Console.WriteLine(Lat);
                    Console.WriteLine(msg);
                }
            }
            ns.Close();
            _client.Close();
            listener.Stop();
        }

        public void splitMsg(string msg)
        {
            string[] splitMs = msg.Split(',');
            Lon = float.Parse(splitMs[0]);//TODO
            Lat = float.Parse(splitMs[1]);
        }
        //readonly Socket sock;
        //public InfoCommunication()
        //{
        //    string ip = FlightSimulator.Properties.Settings.Default.FlightServerIP;
        //    int port = FlightSimulator.Properties.Settings.Default.FlightCommandPort;
        //    sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    //sock.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        //}

        //public void get_string(string path)
        //{
        //    byte[] incomeMsg = new byte[200];
        //    int k = sock.Receive(incomeMsg);
        //    string msg = Encoding.ASCII.GetString(incomeMsg, 0, k);
        //    //string formatter = "set {0}={1}\r\n";
        //    //string message = String.Format(formatter, path, value);
        //    //byte[] bytes = Encoding.ASCII.GetBytes(message);
        //    //sock.Send(bytes);
        //}

        //public void close()
        //{
        //    sock.Close();
        //}
    }
}
