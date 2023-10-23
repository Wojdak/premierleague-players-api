using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PLPlayersAPI.Filters;
using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Services.UserServices;

namespace PLPlayersAPI.Controllers
{
    [Route("api/adminpanel")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private IValidator<UserDTO> _validator;

        public UserController(IUserService userService, IValidator<UserDTO> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var validationResult = await _validator.ValidateAsync(userDTO);

            if (validationResult.IsValid){
                var token = await _userService.LoginUser(userDTO);

                if (token is null)
                    return NotFound("User not found");

                return Ok(token);
            }

            return BadRequest(validationResult.Errors);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var validationResult = await _validator.ValidateAsync(userDTO);

            if (validationResult.IsValid) {
                await _userService.RegisterUser(userDTO);

                return Ok("Account was successfully created");
            }

            return BadRequest(validationResult.Errors);
        }
    }
}
