using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.DBModels
{
    public class Role : IdentityRole<Guid>
    {
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
