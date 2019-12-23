using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.DTO;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public static class TaskRepository
    {
        public static async Task<int> CreateNewTask(int size, int userId)
        {
            var emptyTask = new Models.Task() {Status = "Waiting",DonePercent = (byte) 0, Size = size, UserId = userId};
            using (var dbContext = new LoadManagerContext())
            {
                dbContext.Tasks.Add(emptyTask);
                await dbContext.SaveChangesAsync();
                return emptyTask.TaskID;
            }
        }

        public static async Task AssignTask(int taskId, int? serverId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction()) 
                { 
                    var entity = await dbContext.Tasks.FirstOrDefaultAsync(e => e.TaskID == taskId);
                    entity.ServerID = serverId;
                    entity.Status = "Assigned";
                    dbContext.Tasks.Update(entity);
                    await dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
            }
        }
        public static async Task UnAssignTask(int taskId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction()) 
                { 
                    var entity = await dbContext.Tasks.FirstOrDefaultAsync(e => e.TaskID == taskId);
                    entity.ServerID = null;
                    dbContext.Tasks.Update(entity);
                    await dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
            }
        }

        public static async Task<List<int>> GetAssignedTasks()
        {
            using (var dbContext = new LoadManagerContext())
            {
                return await dbContext.Tasks.Where(t => t.Status.Equals("Assigned"))
                    .Select(p => p.TaskID).ToListAsync();
            }
        }

        public static async Task<List<TaskDto>> GetCanceledTasks()
        {
            using (var dbContext = new LoadManagerContext())
            {
                return await dbContext.Tasks.Where(t => t.Status.Equals("Canceled"))
                    .Select(p => new TaskDto {
                        TaskID = p.TaskID,
                        ServerID = p.ServerID,
                        Status = p.Status,
                        UserId = p.UserId
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
                    Status = e.Status,
                    ServerID = e.ServerID,
                    Size = e.Size
                }).Where(e => e.ServerID == null && e.Status.Equals("Waiting")).ToListAsync();

                return tasks;
            }
        }

        public static async Task<TaskDto> GetTask(int taskId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                return await dbContext.Tasks.Select(x => new TaskDto
                {
                    TaskID = x.TaskID,
                    DonePercent = x.DonePercent,
                    ServerID = x.ServerID,
                    Status = x.Status,
                    Size = x.Size
                }).FirstOrDefaultAsync(x => x.TaskID == taskId);
            }
        }

        public static async Task UpdateTaskStatus(int taskId, string status)
        {
            using (var dbContext = new LoadManagerContext())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    var entity = await dbContext.Tasks.FirstOrDefaultAsync(e => e.TaskID == taskId);
                    entity.Status = status;
                    dbContext.Tasks.Update(entity);
                    await dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
            }
        }

        public static async Task UpdateTaskPercent(int taskId, byte percent)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var entity = await dbContext.Tasks.FirstOrDefaultAsync(e => e.TaskID == taskId);
                entity.DonePercent = percent;
                dbContext.Tasks.Update(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public static async Task<List<TaskDto>> GetWorkingTasks()
        {
            using (var dbContext = new LoadManagerContext())
            {
                var tasks = await dbContext.Tasks.Where(t => t.Status == "Working").Select(t => new TaskDto
                    {
                        TaskID = t.TaskID,
                        Status = t.Status,
                        DonePercent = t.DonePercent,
                        ServerID = t.ServerID,
                        Size = t.Size
                    })
                    .ToListAsync();

                return tasks;
            }
        }

        public static async Task<List<TaskDto>> GetTasks(int userId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var tasks = await dbContext.Tasks.Select(t => new TaskDto
                    {
                        TaskID = t.TaskID,
                        Status = t.Status,
                        DonePercent = t.DonePercent,
                        ServerID = t.ServerID,
                        Size = t.Size,
                        UserId = t.UserId
                    }).Where(i => i.UserId.Equals(userId)).ToListAsync();

                return tasks;
            }
        }
    }
}

