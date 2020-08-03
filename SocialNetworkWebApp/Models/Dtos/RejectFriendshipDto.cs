using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class RejectFriendshipDto
    {
        [Required]
        public Guid FriendshipId { get; set; }
    }
}
