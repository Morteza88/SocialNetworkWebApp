using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.Dtos
{
    public class UnfriendUserDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
