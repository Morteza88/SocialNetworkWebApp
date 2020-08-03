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

        public FriendshipController(IFriendshipService service)
        {
            _service = service;
        }

        // POST: api/Friendship/SendFriendshipRequest
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> SendFriendshipRequest(CreateFriendshipDto createFriendshipDto)
        {
            var result = await _service.CreateFriendshipAsync(createFriendshipDto.UserId);
            if (result != 1)
                return BadRequest();

            return Ok();
        }

        // GET: api/Friendship/GetSentFriendShips
        [HttpGet("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<SentFriendshipDto>>> GetSentFriendShips()
        {
            var sentFriendships = await _service.GetSentFriendshipsAsync();
            if (sentFriendships == null)
                return BadRequest();
            var sentFriendshipDtos = new List<SentFriendshipDto>();
            foreach (var item in sentFriendships)
            {
                sentFriendshipDtos.Add(new SentFriendshipDto
                {
                    Id = item.Id,
                    ToUserId = item.ToUserId,
                    ToUserName = item.ToUser.UserName,
                    State = item.State
                });
            }
            return Ok(sentFriendshipDtos);
        }

        // GET: api/Friendship/GetReceivedFriendships
        [HttpGet("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<ReceivedFriendshipDto>>> GetReceivedFriendships()
        {
            var receivedFriendships = await _service.GetReceivedFriendshipsAsync();
            if (receivedFriendships == null)
            {
                return BadRequest();
            }
            var receivedFriendshipDtos = new List<ReceivedFriendshipDto>();
            foreach (var item in receivedFriendships)
            {
                receivedFriendshipDtos.Add(new ReceivedFriendshipDto 
                { 
                    Id = item.Id, 
                    FromUserId = item.FromUserId,
                    FromUserName = item.FromUser.UserName,
                    State = item.State 
                });
            }
            return Ok(receivedFriendshipDtos);
        }

        // Post: api/Friendship/AcceptFriendship
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AcceptFriendship(AcceptFriendshipDto acceptFriendshipDto)
        {
            var result = await _service.AcceptFriendshipAsync(acceptFriendshipDto.FriendshipId);
            if (result != 1)
                return BadRequest();
            
            return Ok();
        }

        // Post: api/Friendship/RejectFriendship
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> RejectFriendship(RejectFriendshipDto rejectFriendshipDto)
        {
            var result = await _service.RejectFriendshipAsync(rejectFriendshipDto.FriendshipId);
            if (result != 1)
                return BadRequest();
            
            return Ok();
        }

        // Post: api/Friendship/CancelFriendship
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> CancelFriendship(CancelFriendshipDto cancelFriendshipDto)
        {
            var result = await _service.CancelFriendshipAsync(cancelFriendshipDto.FriendshipId);
            if (result != 1)
                return BadRequest();
            
            return Ok();
        }

        // GET: api/Friendship/GetFriends
        [HttpGet("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetFriends()
        {
            var friends = await _service.GetFriendsAsync();
            var friendDtos = new List<UserDto>();
            foreach (var item in friends)
            {
                friendDtos.Add(new UserDto
                {
                    Id = item.Id,
                    FullName = item.FullName,
                });
            }
            return Ok(friendDtos);
        }

        // Post: api/Friendship/Unfriend
        [HttpPost("[action]")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Unfriend(UnfriendUserDto unfriendUserDto)
        {
            var result = await _service.UnfriendAsync(unfriendUserDto.UserId);
            if (result != 1)
                return BadRequest();
            
            return Ok();
        }
    }
}
