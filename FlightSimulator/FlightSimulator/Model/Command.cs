using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class Command
    {
        private TcpClient client = null;
        private NetworkStream stream = null;
        //private readonly static object locker = new object();
        private static Command self = null;

        /**
         * private CTOR, as part of the Singleton design pattern.
         * */
        private Command(){}

        //public void Open(string ip, int port)
        //{
        //    client = new TcpClient(ip, port);
        //    stream = client.GetStream();
        //    stream.Flush();
        //    Console.WriteLine("connected, " + ip + " " + port.ToString());
        //}

        /**
         * closes the client and the network stream.
         * */
        public void Close()
        {
            stream.Close();
            client.Close();
        }

        /**
         * sends the manual input commands to the server.
         * */
        public void ManualSend(string path, double value)
        {
            if (client != null)
            {
                // get the path of the property that was changed.
                string formatter = "set {0}={1}\r\n";
                string message = String.Format(formatter, path, value);
                byte[] bytes = Encoding.ASCII.GetBytes(message);
                //stream.Write(bufbytesfer, 0, bytes.Length);
                Console.WriteLine("command: " + formatter);
                stream.Flush();
                //    sock.Send(bytes);
                //string path = paths[cmd] + value.ToString("N5") + "\r\n";
                //Sender(path);
            }
        }

        /**
         * Sends all commands to the server, waiting two seconds between commands.
         * */
        public void Send(List<string> cmds)
        {
            if (null == client) { return; }
            Thread thread = new Thread(() =>
            {
                foreach (string command in cmds)
                {
                    string cmd = command + "\r\n";
                    //Sender(cmd);
                    Thread.Sleep(2000);
                }
            });
            thread.Start();
        }
        public bool IsExists()
        {
            return this.client != null;
        }

    }
}
