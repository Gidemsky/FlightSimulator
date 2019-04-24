
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

        private bool isShouldStop;

        static class Constants
        {
            public const int LONGTITUDE = 0;
            public const int LATITUDE = 1;
        }

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
                            string incomingInfo = Encoding.ASCII.GetString(bytes);
                            infoRecivedSplitter(incomingInfo);
                            //Console.WriteLine("Incoming position information:");
                            //Console.WriteLine(incomingInfo);
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
            FlightBoardViewModel.Instance.Lon = float.Parse(infosplited[Constants.LONGTITUDE]);
            FlightBoardViewModel.Instance.Lat = float.Parse(infosplited[Constants.LATITUDE]);
        }
    }
}