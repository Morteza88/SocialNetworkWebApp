using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> CreateUserAsync(CreateUserDto createUserDto);
        Task<string> AuthenticateAsync(LoginRequest loginRequest);
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetCurrentUserAsync();
    }
}
