using System;

namespace SocialNetworkWebApp.Models.Contracts
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
    }
}
