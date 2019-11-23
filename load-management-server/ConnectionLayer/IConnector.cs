using System.Threading.Tasks;

namespace ConnectionLayer
{
    public interface IConnector
    {
        Task<bool> EstablishConnection(string ipAddress, int port = 9915);

        void SendBytes(byte[] bytes, string ipAddress, int port = 9915);

        void ReceiveBytes(ref byte[] buffer, string ipAddress, int port = 9915);

        bool Disconnect(string ipAddress, int port);
    }
}
