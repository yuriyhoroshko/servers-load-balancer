using System;
using System.Collections.Generic;
using System.Text;

namespace ConnectionLayer
{
    public interface IConnector
    {
        bool EstablishConnection(string ipAddress, int port = 9915);

        bool SendBytes(byte[] bytes);

        bool Disconnect(string ipAddress);
    }
}
