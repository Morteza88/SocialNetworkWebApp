using Microsoft.EntityFrameworkCore;
using SocialNetworkWebApp.Data;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Enums;
using SocialNetworkWebApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Repositories
{
    public class FriendshipRepository : Repository<Friendship>, IFriendshipRepository
    {
        public FriendshipRepository(SocialNetworkDBContext context) : base(context) { }
        public async Task<IEnumerable<Friendship>> GetFriendRequestsByFromUserAndStateAsync(Guid fromUserId, FriendshipState state)
        {
            return await entities.Where(friendRequest => friendRequest.FromUserId == fromUserId && friendRequest.State == state)
                .Include(friendRequest => friendRequest.ToUser).ToListAsync();
        }
        public async Task<IEnumerable<Friendship>> GetFriendRequestsByToUserAndStateAsync(Guid toUserId, FriendshipState state)
        {
            return await entities.Where(friendRequest => friendRequest.ToUserId == toUserId && friendRequest.State == state)
                .Include(friendRequest => friendRequest.FromUser).ToListAsync();
        }
        public async Task<Friendship> GetFriendRequestsByFromUserAndToUserAsync(Guid fromUserId, Guid toUserId)
        {
            return await entities.FirstOrDefaultAsync(friendRequest => friendRequest.FromUserId == fromUserId
                                                        && friendRequest.ToUserId == toUserId);
        }
    }
}
