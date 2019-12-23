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

            int[,] array1 = new int[22,22]
            {
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5},
                {1,2,3,4,5,6,7,8,9,10,2,3,2,1,5,3,1,2,4,2,1,5}
            };
            int[,] array2 = new int[,]
            {
                {213,123,123},
                {123,123,564},
                {123,342,186}
            };
            //IConnector connector = Connector.GetInstance();
            ComputeTaskService taskService = new ComputeTaskService();

            //Console.WriteLine("Write IP of server (localhost set)");
            //byte[] buffer = new byte[1024];
            //string ipAddress = "localhost";//Console.ReadLine();
            //taskService.StoreTask(array1);
            //if (await connector.EstablishConnection(ipAddress, 9915))
            //{
            //    Console.WriteLine("Connection established, writing to DB...");
            //    //await WorkerRepository.AddNewWorkerServer(new WorkerServerDto() {IpAddress = ipAddress, IsConnected = true, Port = 9915});
            //}

            Console.ReadLine();
            return 1;
        }
    }
}
