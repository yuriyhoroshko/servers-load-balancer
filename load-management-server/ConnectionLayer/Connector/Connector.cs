using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Data.DTO;
using Data.Repository;

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

        public void ConnectToExistingServer(string ipAddress, int port = 9915)
        {
            if (!connectionsDictionary.ContainsKey(ParseIpPort(ipAddress, port)))
            {
                var socketConnection = _connectionFactory.GenerateConnection(ipAddress, port);
                if (socketConnection != null)
                {
                    connectionsDictionary.Add(ParseIpPort(ipAddress, port), socketConnection);
                }
            }
        }

        public async Task<bool> EstablishConnection(string ipAddress, int port = 9915)
        {
            try
            {
                connectionsDictionary.Add(ParseIpPort(ipAddress,port), _connectionFactory.GenerateConnection(ipAddress, port));
                await WorkerRepository.AddNewWorkerServer(new WorkerServerDto
                {
                    IpAddress = ipAddress,
                    IsConnected = true,
                    Port = port
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsConnected(string ipAddress, int port = 9915)
        {
            return connectionsDictionary.ContainsKey(ParseIpPort(ipAddress, port));
        }

        public void SendBytes(byte[] bytes, string ipAddress, int port = 9915)
        {
            try
            {
                connectionsDictionary[ParseIpPort(ipAddress, port)].Send(bytes);
            }
            catch(KeyNotFoundException) {}
        }

        public void ReceiveBytes(ref byte[] buffer, string ipAddress, int port = 9915)
        {
            try
            {
                connectionsDictionary[ParseIpPort(ipAddress, port)].Receive(buffer);
            }
            catch(KeyNotFoundException) { }
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
        private KeyValuePair<string,int> ParseIpPort(string ipAddressWithPort)
        {
            string[] pieces = ipAddressWithPort.Split(':');
            return new KeyValuePair<string, int>(pieces[0],Int32.Parse(pieces[1]));
        }
    }
}