using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace ConnectionLayer.Connector
{
    public class ConnectionFactory
    {
        private readonly byte[] _connectingByte = { 101 };
        public Socket GenerateConnection(string ipAddress, int port)
        {
            Socket socketConnection =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var response = new byte[1];
            socketConnection.Connect(ipAddress, port);
            socketConnection.Send(_connectingByte);
            socketConnection.Receive(response);
            if (response[0] == 111)
            {
                return socketConnection;
            }
            throw new NetworkInformationException();
        }
    }
}
