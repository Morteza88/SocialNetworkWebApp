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
        public async Task<IEnumerable<Friendship>> GetByFromUserAndStateAsync(Guid fromUserId, FriendshipState state)
        {
            return await entities.Where(friendship => friendship.FromUserId == fromUserId && friendship.State == state)
                .Include(friendship => friendship.ToUser).ToListAsync();
        }
        public async Task<IEnumerable<Friendship>> GetByToUserAndStateAsync(Guid toUserId, FriendshipState state)
        {
            return await entities.Where(friendship => friendship.ToUserId == toUserId && friendship.State == state)
                .Include(friendship => friendship.FromUser).ToListAsync();
        }
        public async Task<Friendship> GetByFromUserAndToUserAsync(Guid fromUserId, Guid toUserId)
        {
            return await entities.FirstOrDefaultAsync(friendship => friendship.FromUserId == fromUserId
                                                        && friendship.ToUserId == toUserId);
        }
    }
}
