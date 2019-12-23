using System;
using System.Collections.Generic;
using Contract;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Data.DTO;
using Data.Repository;
using ConnectionLayer;
using ConnectionLayer.Connector;
using Contract.ByteDefinition;

namespace Business
{
    public class ComputeTaskService: IComputeTaskService
    {
        private readonly IConnector _connectionService;
        private SemaphoreSlim _locker = new SemaphoreSlim(1);

        public ComputeTaskService()
        {
            _connectionService = Connector.GetInstance();
        }

        public async void StoreTask(int[,] matrix, int userId)
        {
            int taskId = await TaskRepository.CreateNewTask(matrix.GetLength(0),userId);
            
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

        public void StoreTaskResult(int[,] matrix,int taskId)
        {
            if (Directory.Exists(@"C:\Tasks"))
            {
                if (!File.Exists($@"C:\Tasks\{taskId}_result.dat"))
                {
                    using (var writer = new BinaryWriter(File.Open($@"C:\Tasks\{taskId}_result.dat", FileMode.OpenOrCreate)))
                    {
                        writer.Write(ByteConverter.GetBytes(matrix));
                    }
                }
                else return;
            }
            else
            {
                Directory.CreateDirectory(@"C:\Tasks");
                using (var writer = new BinaryWriter(File.Open($@"C:\Tasks\{taskId}_result.dat", FileMode.OpenOrCreate)))
                {
                    writer.Write(ByteConverter.GetBytes(matrix));
                }
            }
        }

        public async Task<int[,]> ReadStoredTask(int taskId)
        {
            if (File.Exists($@"C:\Tasks\{taskId}.dat"))
            {
                var taskDto = await TaskRepository.GetTask(taskId);
                int[,] matrix = new int[taskDto.Size, taskDto.Size];
                using (var reader = new BinaryReader(File.OpenRead($@"C:\Tasks\{taskId}.dat")))
                {
                    ByteConverter.SetMatrixFromBytes(reader.ReadBytes(268435456), ref matrix);
                    return matrix;
                }
            } throw new FileNotFoundException("No such task file found");
        }

        public async Task<int[,]> ReadStoredTaskResult(int taskId)
        {
            if (File.Exists($@"C:\Tasks\{taskId}_result.dat"))
            {
                var taskDto = await TaskRepository.GetTask(taskId);
                int[,] matrix = new int[taskDto.Size, taskDto.Size];
                using (var reader = new BinaryReader(File.OpenRead($@"C:\Tasks\{taskId}_result.dat")))
                {
                    ByteConverter.SetMatrixFromBytes(reader.ReadBytes(268435456), ref matrix);
                    return matrix;
                }
            }
            throw new FileNotFoundException("No such task file found");
        }

        public async Task AssignTask(int taskId)
        {
            await _locker.WaitAsync();
            try
            {
                int? workerId = await WorkerRepository.FindLessLoadedServer();
                var server = workerId != 0
                    ? await WorkerRepository.GetWorkerServer(workerId.Value)
                    : null;

                bool isServerConnected = server != null && _connectionService.IsConnected(server.IpAddress, server.Port);
                if (workerId != 0 && isServerConnected)
                {
                    await TaskRepository.AssignTask(taskId, workerId);
                }
            }
            
            finally
            {
                _locker.Release();
            }

        }

        public async Task<List<TaskDto>> GetUnassignedTasks()
        {
            return await TaskRepository.GetUnassignedTasks();
        }

        public async Task<List<int>> GetIdToSendList()
        {
            return await TaskRepository.GetAssignedTasks();
        }

        public async Task SendTask(int taskId)
        {
            await _locker.WaitAsync();
            try
            {
                int[,] task = await ReadStoredTask(taskId);
                byte[] matrixSize = ByteConverter.GetByteMatrixSize(task);
                byte[] matrix = ByteConverter.GetBytes(task);
                byte[] buffer = new byte[268435456];

                WorkerServerDto server = await WorkerRepository.GetServerByAssignedTaskId(taskId);

                matrixSize = matrixSize.AddPrefix(Bytes.byteDef["size_prefix"]);
                matrix = matrix.AddPrefix(Bytes.byteDef["task_prefix"]);

                _connectionService.SendBytes(matrixSize, server.IpAddress, server.Port);
                _connectionService.ReceiveBytes(ref buffer, server.IpAddress, server.Port);

                _connectionService.SendBytes(matrix, server.IpAddress, server.Port);
                _connectionService.ReceiveBytes(ref buffer, server.IpAddress, server.Port);

                await TaskRepository.UpdateTaskStatus(taskId, "Working");
            }
            finally
            {
                _locker.Release();
            }
        }

        public async Task UpdateTaskProgress(int taskId)
        {
            byte[] buffer = new byte[268435456];
            var server = await WorkerRepository.GetServerByAssignedTaskId(taskId);
            _connectionService.SendBytes(Bytes.type["progress_request"], server.IpAddress, server.Port);
            _connectionService.ReceiveBytes(ref buffer,server.IpAddress,server.Port);
            buffer = buffer.RemovePrefix();
            await TaskRepository.UpdateTaskPercent(taskId,buffer[0]);
        }

        public async Task CollectResultIfReady(int taskId)
        {
            byte[] buffer = new byte[268435456];
            
            var server = await WorkerRepository.GetServerByAssignedTaskId(taskId);
            var task = await TaskRepository.GetTask(taskId);

            var matrix = new int[task.Size, task.Size];

            _connectionService.SendBytes(Bytes.type["collect_data"], server.IpAddress, server.Port);
            _connectionService.ReceiveBytes(ref buffer, server.IpAddress, server.Port);

            if (buffer[0] == Bytes.byteDef["ready_prefix"])
            {
                buffer = buffer.RemovePrefix();

                ByteConverter.SetMatrixFromBytes(buffer,ref matrix);

                StoreTaskResult(matrix,taskId);

                await TaskRepository.UpdateTaskStatus(taskId, "Done");

                await TaskRepository.UnAssignTask(taskId);

            } else if (buffer[0] == Bytes.byteDef["notyet_prefix"]) {
                return;
            }

        }

        public void RemoveTask(int taskId) {
            //REmove also from db
            File.Delete($@"C:\Tasks\{taskId}.dat");
        }

        public async Task<List<TaskDto>> GetTasks(int userId)
        {
            return await TaskRepository.GetTasks(userId);
        }

        public int[,] ParseStringToMatrix(string input)
        {
            string[] rows = input.Split(',');
            int[,] matrix = new int[rows.Length,rows.Length];
            for (int i = 0; i < rows.Length; i++)
            {
                string[] column = rows[i].Split(' ');
                for (int x = 0; x < column.Length; x++)
                {
                    matrix[i, x] = Int32.Parse(column[x]);
                }
            }

            return matrix;
        }

        public string ParseMatrixToStringWithFormattedView(int[,] matrix)
        {
            var sb = String.Empty;
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    sb += (matrix[x, y].ToString() + " ");
                }

            }
            return sb;
        }

        public int[,] CreateRandomMatrix(int size)
        {
            var randomer = new Random();

            int[,] matrix = new int[size,size];

            for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
            {
                matrix[x, y] = randomer.Next(-999, 999);
            }

            return matrix;
        }

        public async Task UpdateTaskStatus(int taskId, string status)
        {
            await TaskRepository.UpdateTaskStatus(taskId, status);
        }

        public async Task<List<TaskDto>> GetCanceledTasks()
        {
            return await TaskRepository.GetCanceledTasks();
        }

        public async Task CancelTask(int taskId)
        {
            var server = await WorkerRepository.GetServerByAssignedTaskId(taskId);
            _connectionService.SendBytes(Bytes.type["cancel_task"], server.IpAddress, server.Port);
            await TaskRepository.UpdateTaskStatus(taskId, "Disposed");
        }
    }
}
