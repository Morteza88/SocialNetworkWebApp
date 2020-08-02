using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Models
{
    public class Role : IdentityRole<Guid>
    {
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
