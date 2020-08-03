using SocialNetworkWebApp.Models.Enums;
using System;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class ReceivedFriendshipDto
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public string FromUserName { get; set; }
        public FriendshipState State { get; set; }
    }
}
