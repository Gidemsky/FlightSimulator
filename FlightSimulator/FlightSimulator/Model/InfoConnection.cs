
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using FlightSimulator.ViewModels;
using System.Threading;

namespace FlightSimulator
{
    /**
     * The Connection for the incoming information: lat and lon
     */
    class InfoConnection : BaseNotify
    {
        private TcpListener serverListener;
        TcpClient client;

        Thread listernerThread;
               
        private static InfoConnection serverListenerInstance = null;

        private float lon, lat;

        private bool isShouldStop;

        private InfoConnection()
        {
            isShouldStop = false;
        }

        /**
         * The Instance property of the singelton patten
         * */
        public static InfoConnection Instance
        {
            get
            {
                if (serverListenerInstance == null)
                {
                    serverListenerInstance = new InfoConnection();
                }
                return serverListenerInstance;
            }
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

        /**
         * The connection function of the server.
         * this function connects the simulator to this server
         * in order to send the information we need from the client - 
         * the simulator
         */
        public void infoCreateConnection(string ip, int port)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                serverListener = new TcpListener(ep);
                serverListener.Start();
                Console.WriteLine("Connecting to client ip:{0} , on port:{1}", ip, port.ToString());
                Console.WriteLine("Waiting for client's connections...");
                client = serverListener.AcceptTcpClient();
                Console.WriteLine("Client connected!!!");
                //the Lambda ecpression of the reciving information
                listernerThread = new Thread(() =>
                {
                    Byte[] bytes;
                    NetworkStream ns = client.GetStream();
                    while (!isShouldStop)
                    {
                        if (client.ReceiveBufferSize > 0)
                        {
                            bytes = new byte[client.ReceiveBufferSize];
                            ns.Read(bytes, 0, client.ReceiveBufferSize);
                            //TODO: Encoding.UTF8.GetString(, 0, );
                            string incomingInfo = Encoding.ASCII.GetString(bytes);
                            infoRecivedSplitter(incomingInfo);
                            Console.WriteLine("Incoming position information:");
                            Console.WriteLine("Lon {0}", Lon);
                            Console.WriteLine("Lat {0}", Lat);
                            Console.WriteLine(incomingInfo);
                        }
                    }
                    ns.Close();
                    client.Close();
                    serverListener.Stop();
                });
                listernerThread.Start();
            } catch (Exception exception)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(exception.Message);
            }
        }

        /**
         * information recieved by the client saperated by this function
         * update the lon and lt variable
         */
        public void infoRecivedSplitter(string info)
        {
            string[] infosplited = info.Split(',');
            Lon = float.Parse(infosplited[0]);//TODO: create const of 1 and 2
            Lat = float.Parse(infosplited[1]);
        }

        //public void listen(TcpClient client)
        //{
        //    Byte[] bytes;
        //    NetworkStream ns = client.GetStream();
        //    while (!shouldStop)
        //    {
        //        if (client.ReceiveBufferSize > 0)
        //        {
        //            bytes = new byte[client.ReceiveBufferSize];
        //            ns.Read(bytes, 0, client.ReceiveBufferSize);
        //            string incomingInfo = Encoding.ASCII.GetString(bytes);
        //            infoRecivedSplitter(incomingInfo);
        //            Console.WriteLine("Incoming position information:");
        //            Console.WriteLine("Lon {0}", Lon);
        //            Console.WriteLine("Lat {0}", Lat);
        //            Console.WriteLine(incomingInfo);
        //        }
        //    }
        //    ns.Close();
        //    client.Close();
        //    serverListener.Stop();
        //}

        //public void closeThread()
        //{
        //    threadI.Abort();
        //}

        //public void disConnect()
        //{
        //    shouldStop = true;
        //    _client.Close();
        //}
    }
}