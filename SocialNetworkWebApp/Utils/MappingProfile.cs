using AutoMapper;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Friendship, SentFriendshipDto>();
            CreateMap<Friendship, ReceivedFriendshipDto>();
        }
    }
}
