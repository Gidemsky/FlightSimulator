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
using FlightSimulator.Model;

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
         * Connects the simulator to the server in order to send
         * the information we need from the client (the simulator).
         */
        public void InfoCreateConnection(string ip, int port)
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                serverListener = new TcpListener(ep);
                serverListener.Start();
                Console.WriteLine("Connecting to client. ip:{0}, on port:{1}", ip, port.ToString());
                client = serverListener.AcceptTcpClient();
                Console.WriteLine("Client connected!");
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
                            InfoReceivedSplitter(incomingInfo);
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
         * Splits the string with the data that recieved from the client,
         * and updates the lon and lt values.
         */
        public void InfoReceivedSplitter(string info)
        {
            string[] infosplited = info.Split(',');
            try
            {
                FlightBoardViewModel.Instance.Lon = float.Parse(infosplited[Constants.LONGTITUDE]);
                FlightBoardViewModel.Instance.Lat = float.Parse(infosplited[Constants.LATITUDE]);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine("Server or the incoming position information - ERROR");
                Console.WriteLine("Closing the Client's connection");
                isShouldStop = true;
                CmdConnection.Instance.SetIsConnected(false);
            }
        }
    }
}