using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using SocialNetworkWebApp.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<User> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                UserName = createUserDto.UserName,
                FullName = createUserDto.FullName,
                Email = createUserDto.Email
            };
            var CreateUserResult = await _userManager.CreateAsync(user, createUserDto.Password);
            if (CreateUserResult != IdentityResult.Success)
            {
                throw new TaskCanceledException(CreateUserResult.Errors.ToString());
            }
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");
            if (addToRoleResult != IdentityResult.Success)
            {
                throw new TaskCanceledException(addToRoleResult.Errors.ToString());
            }
            return user;
        }
        public async Task<string> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Username);
            if (user == null)
                throw new Exception("Invalid Username or Password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isPasswordValid)
                throw new Exception("Invalid Username or Password");

            var jwtToken = await generateJwtToken(user);
            return jwtToken;
        }
        private async Task<string> generateJwtToken(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName) };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<User> GetCurrentUserAsync()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            string userName = null;
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {
                    userName = claim.Value;
                }
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                throw new EntryPointNotFoundException("couldn't find any user with UserName = " + userName);
            }
            return user;
        }
        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new EntryPointNotFoundException("couldn't find any user with Id = " + id);
            }
            return (user);
        }
    }
}
