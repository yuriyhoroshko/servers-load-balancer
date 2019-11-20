using System;
using System.Threading;
using System.Threading.Tasks;
using Business;
using ConnectionLayer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Scheduler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IComputeTaskService _computeTaskService;
        private readonly IConnector _connectionService;

        public Worker(ILogger<Worker> logger, IComputeTaskService computeTaskService, IConnector connectionService)
        {
            _logger = logger;
            _computeTaskService = computeTaskService;
            _connectionService = connectionService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Started jobs at: {time}", DateTimeOffset.Now);

                var unassignedTasks = await _computeTaskService.GetUnassignedTasks();

                unassignedTasks.ForEach(t => _computeTaskService.AssignTask(t.TaskID));
                

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
        }
    }
}
