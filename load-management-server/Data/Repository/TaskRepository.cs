using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.DTO;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public static class TaskRepository
    {
        public static async Task<int> CreateNewTask()
        {
            var emptyTask = new Models.Task() {Status = "Waiting"};
            using (var dbContext = new LoadManagerContext())
            {
                dbContext.Tasks.Add(emptyTask);
                await dbContext.SaveChangesAsync();
                return emptyTask.TaskID;
            }
        }

        public static async void AssignTask(int taskId, int serverId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var entity = await dbContext.Tasks.FirstOrDefaultAsync(e => e.TaskID == taskId);
                entity.ServerID = serverId;
                entity.Status = "Assigned";
                dbContext.Tasks.Update(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task<List<TaskDto>> GetAssignedTasks()
        {
            using (var dbContext = new LoadManagerContext())
            {
                return await dbContext.Tasks.Where(t => t.Status.Equals("Assigned"))
                    .Select(p => new TaskDto
                    {
                        Status = p.Status,
                        ServerID = p.ServerID,
                        TaskID = p.TaskID
                    }).ToListAsync();
            }
        }

        public static async Task<List<TaskDto>> GetUnassignedTasks()
        {
            using (var dbContext = new LoadManagerContext())
            {
                var tasks = await dbContext.Tasks.Select(e => new TaskDto
                {
                    TaskID = e.TaskID,
                    Status = e.Status
                }).Where(e => e.ServerID == null).ToListAsync();

                return tasks;
            }
        }

        public static async void UpdateTaskStatus(int taskId, string status)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var entity = await dbContext.Tasks.FirstOrDefaultAsync(e => e.TaskID == taskId);
                entity.Status = status;
                dbContext.Tasks.Update(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}

