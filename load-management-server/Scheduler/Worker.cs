using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Business;
using ConnectionLayer;
using ConnectionLayer.Connector;
using Data.DTO;
using Data.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Scheduler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IComputeTaskService _computeTaskService;
        private readonly IConnector _connectionService;
        private SemaphoreSlim _locker = new SemaphoreSlim(1);

        public Worker(ILogger<Worker> logger, IComputeTaskService computeTaskService)
        {
            _logger = logger;
            _computeTaskService = computeTaskService;
            _connectionService = Connector.GetInstance();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try 
                {
                        _logger.LogInformation("Started jobs at: {time}", DateTimeOffset.Now);

                        var servers = await WorkerRepository.GetWorkerServer();

                        _logger.LogInformation("Loaded servers: {0}", servers.Count);

                        servers.ForEach(s => _connectionService.ConnectToExistingServer(s.IpAddress, s.Port));

                        var unassignedTasks = await _computeTaskService.GetUnassignedTasks();

                        _logger.LogInformation("Got unassigned tasks: {0}", unassignedTasks.Count);

                        await unassignedTasks.ForEachAsync(async t => await _computeTaskService.AssignTask(t.TaskID));

                        var taskToCancel = await _computeTaskService.GetCanceledTasks();

                        await taskToCancel.ForEachAsync(async t => await _computeTaskService.CancelTask(t.TaskID));

                        var assignedTasksId = await _computeTaskService.GetIdToSendList();

                        _logger.LogInformation("Got assigned tasks: {0}", assignedTasksId.Count);

                        await assignedTasksId.ForEachAsync(async t => await _computeTaskService.SendTask(t));

                        var workingTasks = await TaskRepository.GetWorkingTasks();

                        await workingTasks.ForEachAsync(async t => await _computeTaskService.UpdateTaskProgress(t.TaskID));

                        _logger.LogInformation("Updated tasks progress");

                        await workingTasks.ForEachAsync(async t => await _computeTaskService.CollectResultIfReady(t.TaskID));

                        _logger.LogInformation("Checked tasks results");
                        _locker.Release();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
                await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
            }
        }
    }
}
