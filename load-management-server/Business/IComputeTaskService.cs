using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.DTO;

namespace Business
{
    public interface IComputeTaskService
    {
        void StoreTask(int[,] matrix, int userId);

        Task SendTask(int taskId);

        void RemoveTask(int taskId);

        Task<List<TaskDto>> GetUnassignedTasks();

        void StoreTaskResult(int[,] matrix, int taskId);

        Task<int[,]> ReadStoredTask(int taskId);

        Task AssignTask(int taskId);

        Task<List<int>> GetIdToSendList();

        Task UpdateTaskProgress(int taskId);

        Task CollectResultIfReady(int taskId);

        Task<List<TaskDto>> GetTasks(int userId);

        int[,] ParseStringToMatrix(string input);

        string ParseMatrixToStringWithFormattedView(int[,] matrix);

        Task<int[,]> ReadStoredTaskResult(int taskId);

        int[,] CreateRandomMatrix(int size);

        Task UpdateTaskStatus(int taskId, string status);

        Task<List<TaskDto>> GetCanceledTasks();

        Task CancelTask(int taskId);
    }
}
