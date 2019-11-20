using System;
using System.Threading.Tasks;
using ConnectionLayer;
using ConnectionLayer.Connector;
using Contract;
using Contract.ByteDefinition;
using Data.DTO;
using Data.Repository;
using Business;

namespace ConsoleRunner
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            ComputeTaskService taskService = new ComputeTaskService();

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
            byte[] buffer = new byte[1024];
            string ipAddress = "localhost";//Console.ReadLine();
            taskService.StoreTask(array2);
            if (connector.EstablishConnection(ipAddress))
            {
                Console.WriteLine("Connection established, writing to DB...");
                await WorkerRepository.AddNewWorkerServer(new WorkerServerDto() {IpAddress = ipAddress, IsConnected = true, Port = 9915});
                connector.SendBytes(ByteConverter.GetByteMatrixSize(array2).AddPrefix(Bytes.byteDef["size_prefix"]));
                connector.ReceiveBytes(ref buffer);
                if (buffer[0] == Bytes.type["size_accepted"][0])
                {
                    Console.WriteLine("Size transferred");
                }
                connector.SendBytes(ByteConverter.GetBytes(array2).AddPrefix(Bytes.byteDef["task_prefix"]));
                connector.ReceiveBytes(ref buffer);
                if (buffer[0] == Bytes.type["matrix_accepted"][0])
                {
                    Console.WriteLine("Matrix transferred");
                }
            }

            return 1;
        }
    }
}
