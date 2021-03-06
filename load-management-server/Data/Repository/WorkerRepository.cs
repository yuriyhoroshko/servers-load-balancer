﻿using System.Collections.Generic;
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
        public static async Task<int?> FindLessLoadedServer()
        {
            using (var dbContext = new LoadManagerContext())
            {
                int? serverId = await dbContext.WorkerServers.Select(b => b.WorkerServerID).Where(b =>
                    dbContext.Tasks.Count(p => p.ServerID == b) == 0).FirstOrDefaultAsync();

                return serverId?? 0;
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
                    }).GroupBy(g => g.ServerID).Select(p => new WorkerTaskDto
                    {
                        IpAddress = p.Select(p=>p.IpAddress).First(),
                        TaskCount = p.Count(),
                        IsConnected = p.Select(p=>p.IsConnected).First(),
                        Port = p.Select(p=> p.Port).First()
                    }).OrderBy(p => p.TaskCount).ToListAsync();
                return serverList;
            }
        }

        public static async Task<WorkerServerDto> GetServerByAssignedTaskId(int taskId)
        {
            using (var dbContext = new LoadManagerContext())
            {
                var serverId = await dbContext.Tasks.Where(p => p.TaskID == taskId).Select(p => p.ServerID).FirstAsync();

                var server = await dbContext.WorkerServers.Where(p => p.WorkerServerID == serverId).Select(s =>
                    new WorkerServerDto
                    {
                        IpAddress = s.IpAddress,
                        IsConnected = s.IsConnected,
                        Port = s.Port,
                        WorkerServerID = s.WorkerServerID
                    }).FirstAsync();
                //    new {p.ServerID, p.TaskID}).Where(p => p.TaskID == taskId)
                //    .LeftJoin(dbContext.WorkerServers, p => p.ServerID, o => o.WorkerServerID, (a, g) => new
                //    {
                //        a.TaskID,
                //        g.IpAddress,
                //        g.IsConnected,
                //        g.Port,
                //        g.WorkerServerID
                //    }).GroupBy(p => p.WorkerServerID).Select(p =>
                //new WorkerServerDto {
                //    IpAddress = p.IpAddress,
                //    IsConnected = p.IsConnected,
                //    Port = p.Port,
                //    WorkerServerID = p.WorkerServerID
                //}).FirstAsync();

                return server;
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
                    IsConnected = server.IsConnected,
                    Port = server.Port
                });
                return await dbContext.SaveChangesAsync() == 1;
            }
        }
    }
}
