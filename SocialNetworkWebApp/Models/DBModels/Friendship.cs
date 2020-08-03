using SocialNetworkWebApp.Models.Enums;
using System;

namespace SocialNetworkWebApp.Models.DBModels
{
    public class Friendship : BaseEntity
    {
        public Guid FromUserId { get; set; }
        public User FromUser { get; set; }
        public Guid ToUserId { get; set; }
        public User ToUser { get; set; }
        public FriendshipState State { get; set; }
    }
}
