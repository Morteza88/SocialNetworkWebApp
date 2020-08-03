using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class AcceptFriendshipDto
    {
        [Required]
        public Guid FriendshipId { get; set; }
    }
}
