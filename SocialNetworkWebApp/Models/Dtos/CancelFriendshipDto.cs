using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class CancelFriendshipDto
    {
        [Required]
        public Guid FriendshipId { get; set; }
    }
}
