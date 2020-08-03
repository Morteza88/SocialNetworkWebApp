using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using System.Collections.Generic;
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
