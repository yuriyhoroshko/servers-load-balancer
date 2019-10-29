using System;
using ConnectionLayer;
using ConnectionLayer.Connector;
using Business;
namespace ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] array1 = new int[,]
            {
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
                {1,2,3,4,5,6,7,8,9,10},
            };
            int[,] array2 = new int[,]
            {
                {213,123,123},
                {123,123,564},
                {123,342,186}
            };
            IConnector connector = new Connector();
            Console.WriteLine("Write IP of server (localhost set)");

            string ipAddress = "localhost";//Console.ReadLine();
            if (connector.EstablishConnection(ipAddress))
            {
                Console.WriteLine("Connection established, waiting for command");
                connector.SendBytes(ByteConverter.GetBytes(array1).AddPrefix((byte)153));
            }
            
        }
    }
}
