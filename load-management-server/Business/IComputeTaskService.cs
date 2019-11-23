using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.DTO;

namespace Business
{
    public interface IComputeTaskService
    {
        void StoreTask(int[,] matrix);

        void SendTask(int taskId);

        void RemoveTask(int taskId);

        Task<List<TaskDto>> GetUnassignedTasks();

        int[,] ReadStoredTask(int taskId);

        void AssignTask(int taskId);

        Task<List<int>> GetIdToSendList();

        void UpdateTaskProgress(int taskId);
    }
}
