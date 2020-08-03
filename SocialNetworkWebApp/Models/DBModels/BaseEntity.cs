using SocialNetworkWebApp.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWebApp.Models.DBModels
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
