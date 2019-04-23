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
        TcpClient client;
        Thread ThreadC;

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
                client = tcpListener.AcceptTcpClient();
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

        public void Send_string(string path, double value)
        {
            string formatter = "set {0}={1}\r\n";
            string[] commands = prep(formatter);
            string message = String.Format(formatter, path, value);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            ThreadC = new Thread(() => sendMessage(commands, client));
            //sock.Send(bytes);
        }

        private string[] prep(string line)
        {
            string[] newLine = { "\r\n" };
            string[] input = line.Split(newLine, StringSplitOptions.None);
            return input;
        }

        public void sendMessage(string[] splited, TcpClient tcpClient)
        {
            if (!isConnceted)
            {
                return;
            }
            NetworkStream ns = tcpClient.GetStream();
            foreach (string split in splited)
            {
                // Send data to server
                Console.Write("Please enter a number: ");
                string command = split;
                //string num = Console.ReadLine();
                command += "\r\n";
                byte[] buffer = Encoding.ASCII.GetBytes(command);
                ns.Write(buffer, 0, buffer.Length);
                Thread.Sleep(2000);
                //writer.Write(num);
                //writer.Flush();
                // Get result from server
                //string result = reader.ReadLine();
                //Console.WriteLine("Result = {0}", result);
            }
            //tcpClient.Close();
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
