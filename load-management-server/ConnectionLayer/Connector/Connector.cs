using System.Net.Sockets;

namespace ConnectionLayer.Connector
{
    public class Connector : IConnector
    {
        private readonly byte[] _connectingByte = {101};

        private readonly Socket _socketConnection =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public bool EstablishConnection(string ipAddress, int port = 9915)
        {
            try
            {
                var response = new byte[1];
                _socketConnection.Connect(ipAddress, port);
                _socketConnection.Send(_connectingByte);
                _socketConnection.Receive(response);
                return response[0] == 111;
            }
            catch
            {
                return false;
            }
        }

        public bool SendBytes(byte[] bytes)
        {
            var buffer = new byte[1];
            _socketConnection.Send(bytes);
            _socketConnection.Receive(buffer);
            return buffer[0] == 191;
        }

        public bool Disconnect(string ipAddress)
        {
            return true;
        }
    }
}