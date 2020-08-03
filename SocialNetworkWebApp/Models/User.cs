using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Models
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            IsActive = true;
        }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Friendship> SentRequests { get; set; }
        public ICollection<Friendship> ReceivedRequests { get; set; }
    }
}
