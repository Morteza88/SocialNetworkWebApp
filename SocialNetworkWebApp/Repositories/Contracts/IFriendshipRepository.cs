using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Repositories.Contracts
{
    public interface IFriendshipRepository : IRepository<Friendship>
    {
        Task<IEnumerable<Friendship>> GetFriendRequestsByFromUserAndStateAsync(Guid fromUserId, FriendshipState state);
        Task<IEnumerable<Friendship>> GetFriendRequestsByToUserAndStateAsync(Guid toUserId, FriendshipState state);
        Task<Friendship> GetFriendRequestsByFromUserAndToUserAsync(Guid fromUserId, Guid toUserId);
    }
}
