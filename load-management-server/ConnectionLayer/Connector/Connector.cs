using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace ConnectionLayer.Connector
{
    public class Connector : IConnector
    {
        private readonly byte[] _connectingByte = {101};

        private static Connector instanse;

        private readonly ConnectionFactory _connectionFactory = new ConnectionFactory();

        private Connector() { }

        public static Connector GetInstance()
        {
            return instanse ??= new Connector();
        }

        private Dictionary<String, Socket> connectionsDictionary = new Dictionary<string, Socket>();

        public bool EstablishConnection(string ipAddress, int port = 9915)
        {
            try
            {
                connectionsDictionary.Add(ParseIpPort(ipAddress,port), _connectionFactory.GenerateConnection(ipAddress, port));
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SendBytes(byte[] bytes, string ipAddress, int port = 9915)
        {
            connectionsDictionary[ParseIpPort(ipAddress,port)].Send(bytes);
        }

        public void ReceiveBytes(ref byte[] buffer, string ipAddress, int port = 9915)
        {
            connectionsDictionary[ParseIpPort(ipAddress, port)].Receive(buffer);
        }

        public bool Disconnect(string ipAddress, int port = 9915)
        {
            connectionsDictionary[ParseIpPort(ipAddress, port)].Disconnect(true);
            return true;
        }

        private string ParseIpPort(string ipAddress, int port)
        {
            return $"{ipAddress}:{port}";
        }
    }
}