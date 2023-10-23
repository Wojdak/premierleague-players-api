using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.UserServices;

namespace PLPlayersAPI.Controllers
{
    [Route("api/adminpanel")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var token = await _userService.LoginUser(userDTO);

            if (token is null)
                return NotFound("User not found");

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            await _userService.RegisterUser(userDTO);

            return Ok("Account was successfully created");
        }
    }
}
