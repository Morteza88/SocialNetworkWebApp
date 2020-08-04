using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using SocialNetworkWebApp.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _service;
        private readonly IMapper _mapper;

        public FriendshipController(IFriendshipService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // POST: api/Friendship/SendFriendshipRequest
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> SendFriendshipRequest(CreateFriendshipDto createFriendshipDto)
        {
            await _service.CreateFriendshipAsync(createFriendshipDto.UserId);
            return Ok();
        }

        // GET: api/Friendship/GetSentFriendShips
        [HttpGet("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<SentFriendshipDto>>> GetSentFriendShips()
        {
            var sentFriendships = await _service.GetSentFriendshipsAsync();
            var sentFriendshipDtos = _mapper.Map<IEnumerable<SentFriendshipDto>>(sentFriendships);
            return Ok(sentFriendshipDtos);
        }

        // GET: api/Friendship/GetReceivedFriendships
        [HttpGet("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ReceivedFriendshipDto>>> GetReceivedFriendships()
        {
            var receivedFriendships = await _service.GetReceivedFriendshipsAsync();
            var receivedFriendshipDtos = _mapper.Map<IEnumerable<ReceivedFriendshipDto>>(receivedFriendships);
            return Ok(receivedFriendshipDtos);
        }

        // Post: api/Friendship/AcceptFriendship
        [HttpPut("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AcceptFriendship(AcceptFriendshipDto acceptFriendshipDto)
        {
            await _service.AcceptFriendshipAsync(acceptFriendshipDto.FriendshipId);
            return Ok();
        }

        // Post: api/Friendship/RejectFriendship
        [HttpPut("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RejectFriendship(RejectFriendshipDto rejectFriendshipDto)
        {
            await _service.RejectFriendshipAsync(rejectFriendshipDto.FriendshipId);
            return Ok();
        }

        // Post: api/Friendship/CancelFriendship
        [HttpDelete("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CancelFriendship(CancelFriendshipDto cancelFriendshipDto)
        {
            await _service.CancelFriendshipAsync(cancelFriendshipDto.FriendshipId);
            return Ok();
        }

        // GET: api/Friendship/GetFriends
        [HttpGet("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetFriends()
        {
            var friends = await _service.GetFriendsAsync();
            var friendDtos = _mapper.Map<IEnumerable<UserDto>>(friends);
            return Ok(friendDtos);
        }

        // Post: api/Friendship/Unfriend
        [HttpDelete("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Unfriend(UnfriendUserDto unfriendUserDto)
        {
            await _service.UnfriendAsync(unfriendUserDto.UserId);
            return Ok();
        }
    }
}
