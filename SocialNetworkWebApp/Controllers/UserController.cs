using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using SocialNetworkWebApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            if (users == null)
                return BadRequest();

            return Ok(users);
        }

        // POST: api/Users/Register
        [HttpPost("[action]")]
        public async Task<ActionResult<User>> Register(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            if (user == null)
                return BadRequest();

            return Ok(user);
        }

        // POST: api/Users/Login
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var jwtToken = await _userService.AuthenticateAsync(request);
            if (jwtToken == null)
                return BadRequest();

            return Ok(jwtToken);
        }
    }
}
