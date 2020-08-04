using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using SocialNetworkWebApp.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(userDtos);
        }

        // POST: api/Users/Register
        [HttpPost("[action]")]
        public async Task<ActionResult<UserDto>> Register(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // POST: api/Users/Login
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var jwtToken = await _userService.AuthenticateAsync(request);
            return Ok(jwtToken);
        }
    }
}
