using System;
using System.Threading.Tasks;
using Business;
using Data.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace load_management_server.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserDto requestObj)
        {
            var registration = await _userService.RegisterUserAsync(requestObj);

            return registration != null ? (IActionResult)Ok(JsonConvert.SerializeObject(registration)) : BadRequest();
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserDto loginObject)
        {
            System.Diagnostics.Debugger.Launch();
            var login = await _userService.LoginUserAsync(loginObject);

            return login != null ? (IActionResult)Ok(JsonConvert.SerializeObject(login)) : BadRequest();
        }
    }
}