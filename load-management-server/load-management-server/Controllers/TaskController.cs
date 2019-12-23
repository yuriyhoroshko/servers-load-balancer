using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Mvc;

namespace load_management_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private IComputeTaskService _computeTaskService;

        public TaskController(IComputeTaskService computeTaskService)
        {
            _computeTaskService = computeTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(int userId)
        {
            try
            {
                var tasks = Json(await _computeTaskService.GetTasks(userId));

                return Ok(tasks);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult SendTask(int matrixSize, int userId)
        { 
            var matrix = _computeTaskService.CreateRandomMatrix(matrixSize);
            _computeTaskService.StoreTask(matrix, userId);
            return new OkResult();
        }

        [HttpGet]
        [Route("getTaskResult")]
        public async Task<IActionResult> GetTaskResult(int taskId)
        {
            var matrix = await _computeTaskService.ReadStoredTaskResult(taskId);
            return Ok(_computeTaskService.ParseMatrixToStringWithFormattedView(matrix));
        }

        [HttpPost]
        [Route("cancelTask")]
        public async Task<IActionResult> CancelTask(int taskId)
        {
            try
            {
                await _computeTaskService.UpdateTaskStatus(taskId, "Canceled");
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}