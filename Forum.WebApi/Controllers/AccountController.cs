using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.Models.UserManagement;
using Forum.Services.UserManagement;
using Forum.WebApi.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterUserRequest registerUser)
        {
            await _userService.CreateUserAccount(registerUser);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login([FromBody] LoginUserRequest loginUser)
        {
            var user = await _userService.AuthenticateUser(loginUser);
            if (user == null)
            {
                return BadRequest("unregistered user");
            }

            return Ok(user);
        }

        [Authorize]
        [HttpGet("Get")]
        public async Task<ActionResult<UserResponse>> Login()
        {
           

            return Ok(new {id = "Object"});
        }
    }
}