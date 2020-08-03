using SocialNetworkWebApp.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Services.Contracts
{
    public interface IFriendshipService
    {
        Task<int> CreateFriendshipAsync(Guid userId);
        Task<IEnumerable<Friendship>> GetSentFriendshipsAsync();
        Task<IEnumerable<Friendship>> GetReceivedFriendshipsAsync();
        Task<int> AcceptFriendshipAsync(Guid id);
        Task<int> RejectFriendshipAsync(Guid id);
        Task<int> CancelFriendshipAsync(Guid id);
        Task<IEnumerable<User>> GetFriendsAsync();
        Task<int> UnfriendAsync(Guid userId);
    }
}
