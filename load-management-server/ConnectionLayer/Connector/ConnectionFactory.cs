using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConnectionLayer.Connector
{
    public class ConnectionFactory
    {
        private readonly byte[] _connectingByte = { 101 };

        public Socket GenerateConnection(string ipAddress, int port)
        {
            int retries = 5;
            Socket socketConnection =
                new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var response = new byte[1];

            try
            {
                int retr = 0;
                while (retr < retries)
                {
                    socketConnection.Connect(ipAddress, port);
                    if (socketConnection.Connected)
                    {
                        socketConnection.Send(_connectingByte);
                        socketConnection.Receive(response);
                        if (response[0] == 111)
                        {
                            return socketConnection;
                        }


                        throw new NetworkInformationException();
                    }

                    retr++;
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
