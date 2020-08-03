using SocialNetworkWebApp.Models.Enums;
using System;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class SentFriendshipDto
    {
        public Guid Id { get; set; }
        public Guid ToUserId { get; set; }
        public string ToUserName { get; set; }
        public FriendshipState State { get; set; }
    }
}
