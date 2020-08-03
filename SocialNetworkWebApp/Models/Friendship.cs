using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Models
{
    public class Friendship : BaseEntity
    {
        public Guid FromUserId { get; set; }
        public User FromUser { get; set; }
        public Guid ToUserId { get; set; }
        public User ToUser { get; set; }
        public FriendshipState State { get; set; }
    }
}
