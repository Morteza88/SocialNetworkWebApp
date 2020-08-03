using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class CreateFriendshipDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
