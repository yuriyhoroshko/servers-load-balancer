using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.DTO;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Task = System.Threading.Tasks.Task;

namespace Data.Repository
{
    public static class WorkerRepository
    {
        public static async Task<int> FindLessLoadedServer()
        {
            using (var dbContext = new LoadManagerContext())
            {
                var serverList = await dbContext.WorkerServers.Select(b => new {b.WorkerServerID})
                    .LeftJoin(dbContext.Tasks, p => p.WorkerServerID, s => s.ServerID, (a, g) => new
                    {
                        g.Status,
                        g.ServerID
                    }).Where(p => p.Status != "Done").GroupBy(g => g.ServerID).Select(p => new
                    {
                        WorkerServerId = p.Key,
                        count = p.Count()
                    }).OrderBy(p => p.count).ToListAsync();
                return serverList[0].WorkerServerId.GetValueOrDefault(-1);
            }
        }

        public static async Task<List<WorkerTaskDto>> FindAllServers()
        {
            using (var dbContext = new LoadManagerContext())
            {
                var serverList = await dbContext.WorkerServers.Select(b => new { b.WorkerServerID, b.IpAddress, b.IsConnected, b.Port })
                    .LeftJoin(dbContext.Tasks, p => p.WorkerServerID, s => s.ServerID, (a, g) => new
                    {
                        g.ServerID,
                        a.IpAddress,
                        a.IsConnected,
                        a.Port
                    }).GroupBy(g => g.ServerID).Select(p => new WorkerTaskDto()
                    {
                        IpAddress = p.Select(p=>p.IpAddress).First(),
                        TaskCount = p.Count(),
                        IsConnected = p.Select(p=>p.IsConnected).First(),
                        Port = p.Select(p=> p.Port).First()
                    }).OrderBy(p => p.TaskCount).ToListAsync();
                return serverList;
            }
        }

        public static async Task<WorkerServerDto> GetWorkerServer(int workerId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var server = await dbContext.WorkerServers.Select(p => new WorkerServerDto()
                {
                    WorkerServerID = p.WorkerServerID,
                    Port = p.Port,
                    IpAddress = p.IpAddress,
                    IsConnected = p.IsConnected
                }).Where(p => p.WorkerServerID == workerId).ToListAsync();

                return server[0];
            }
        }

        public static async Task<List<WorkerServerDto>> GetWorkerServer()
        {
            using (var dbContext = new LoadManagerContext())
            {
                var server = await dbContext.WorkerServers.Select(p => new WorkerServerDto()
                {
                    WorkerServerID = p.WorkerServerID,
                    Port = p.Port,
                    IpAddress = p.IpAddress,
                    IsConnected = p.IsConnected
                }).ToListAsync();

                return server;
            }
        }

        public static async Task<bool> AddNewWorkerServer(WorkerServerDto server)
        {
            using (var dbContext = new LoadManagerContext())
            {
                if (dbContext.WorkerServers.Count(b => b.IpAddress == server.IpAddress && b.Port == server.Port) != 0)
                {
                    return true;
                }

                dbContext.WorkerServers.Add(new WorkerServer()
                {
                    IpAddress = server.IpAddress,
                    IsConnected = server.IsConnected
                });
                return await dbContext.SaveChangesAsync() == 1;
            }
        }
    }
}
