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
        Task<IEnumerable<Friendship>> GetByFromUserAndStateAsync(Guid fromUserId, FriendshipState state);
        Task<IEnumerable<Friendship>> GetByToUserAndStateAsync(Guid toUserId, FriendshipState state);
        Task<Friendship> GetByFromUserAndToUserAsync(Guid fromUserId, Guid toUserId);
    }
}
