using SocialNetworkWebApp.Models;
using SocialNetworkWebApp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> CreateUserAsync(CreateUserDto createUserDto);
        Task<string> AuthenticateAsync(LoginRequest loginRequest);
    }
}
