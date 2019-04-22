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
    class ServerConnect : BaseNotify
    {
        private static ServerConnect self;
        private TcpListener tcpListener = null;
        bool isConnceted;
        private string socketData;

        public static ServerConnect Instance
        {
            get
            {
                if (self == null)
                {
                    self = new ServerConnect();
                }
                return self;
            }
        }

        public string Data
        {
            get
            {
                return socketData;
            }
            set
            {
                socketData = value;
                NotifyPropertyChanged("Data");
            }
        }

        public void createConnection(string ip, int port)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                tcpListener = new TcpListener(ep);
                tcpListener.Start();
                Console.WriteLine("Waiting for incoming connections...");//TODO: erase or change the msg
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Accepted Client !");//TODO: erase or change the msg
                NetworkStream stream = client.GetStream();
                Thread t = new Thread(() => {
                    while (!isConnceted)
                    {
                        if (stream.DataAvailable)
                        {
                            byte[] line = new byte[512];//TODO: create constant
                            int read = stream.Read(line, 0, line.Length);
                            string parsedData = Encoding.UTF8.GetString(line, 0, read);
                            Data = parsedData;
                        }
                    }
                    stream.Close();
                    client.Close();
                });
                t.Start();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("ERROR");
            }
            finally
            {
                tcpListener.Stop();
            }

        }

        public void Close()
        {
            isConnceted = true;
        }

        public bool IsOpen()
        {
            return this.tcpListener != null;
        }


    }
}
