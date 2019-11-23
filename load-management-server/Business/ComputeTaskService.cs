using System;
using System.Collections.Generic;
using Contract;
using System.IO;
using System.Threading.Tasks;
using Data.DTO;
using Data.Repository;
using ConnectionLayer;

namespace Business
{
    public class ComputeTaskService: IComputeTaskService
    {
        private IConnector _connectionService;

        public ComputeTaskService(IConnector connectionService)
        {
            _connectionService = connectionService;
        }

        public async void StoreTask(int[,] matrix) {
            int taskId = await TaskRepository.CreateNewTask();
            
            if (Directory.Exists(@"C:\Tasks"))
            {
                if (!File.Exists($@"C:\Tasks\{taskId}.dat"))
                {
                    using (var writer = new BinaryWriter(File.Open($@"C:\Tasks\{taskId}.dat", FileMode.OpenOrCreate)))
                    {
                        writer.Write(ByteConverter.GetBytes(matrix));
                    }
                }
                else return;
            }
            else
            {
                Directory.CreateDirectory(@"C:\Tasks");
                using (var writer = new BinaryWriter(File.Open($@"C:\Tasks\{taskId}.dat", FileMode.OpenOrCreate)))
                {
                    writer.Write(ByteConverter.GetBytes(matrix));
                }
            }
        }

        public int[,] ReadStoredTask(int taskId)
        {
            if (File.Exists($@"C:\Tasks\{taskId}.dat"))
            {
                int[,] matrix = { };
                using (var reader = new BinaryReader(File.OpenRead($@"C:\Tasks\{taskId}.dat")))
                {
                    ByteConverter.SetMatrixFromBytes(reader.ReadBytes(int.MaxValue), ref matrix);
                    return matrix;
                }
            } throw new FileNotFoundException("No such task file found");
        }

        public async void AssignTask(int taskId)
        {
            int workerId = await WorkerRepository.FindLessLoadedServer();
            TaskRepository.AssignTask(taskId,workerId);
        }

        public async Task<List<TaskDto>> GetUnassignedTasks()
        {
            return await TaskRepository.GetUnassignedTasks();
        }

        public async Task<List<int>> GetIdToSendList()
        {
            return await TaskRepository.GetAssignedTasks();
        }

        public async void SendTask(int taskId)
        {
            int[,] task = ReadStoredTask(taskId);
            byte[] matrixSize = ByteConverter.GetByteMatrixSize(task);
            byte[] matrix = ByteConverter.GetBytes(task);
            byte[] buffer = {};
            WorkerServerDto server = await WorkerRepository.GetServerByAssignedTaskId(taskId);

            matrixSize.AddPrefix(Contract.ByteDefinition.Bytes.byteDef["size_prefix"]);
            matrix.AddPrefix(Contract.ByteDefinition.Bytes.byteDef["task_prefix"]);

            _connectionService.SendBytes(matrixSize,server.IpAddress,server.Port);
            _connectionService.ReceiveBytes(ref buffer,server.IpAddress,server.Port);

            _connectionService.SendBytes(matrix,server.IpAddress,server.Port);
            _connectionService.ReceiveBytes(ref buffer, server.IpAddress, server.Port);

            TaskRepository.UpdateTaskStatus(taskId,"Working");

        }

        public void RemoveTask(int taskId) {
            //REmove also from db
            File.Delete($@"C:\Tasks\{taskId}.dat");
        }
    }
}
