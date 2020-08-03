using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Enums;
using SocialNetworkWebApp.Repositories.Contracts;
using SocialNetworkWebApp.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.Services
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _repository;
        private readonly IUserService _userService;
        
        public FriendshipService(IFriendshipRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }
        public async Task<int> CreateFriendshipAsync(Guid userId)
        {
            var fromUser = await _userService.GetCurrentUserAsync();
            var toUser = await _userService.GetUserByIdAsync(userId);
            if (fromUser.Id == toUser.Id)
            {
                throw new AccessViolationException("you can't send Friendship to yourself");
            }
            var sentFriendship = await _repository.GetByFromUserAndToUserAsync(fromUser.Id, toUser.Id);
            if (sentFriendship != null)
            {
                if (sentFriendship.State == FriendshipState.Requested)
                {
                    throw new AccessViolationException("you have already sent a Friendship to this user");
                }
                else if (sentFriendship.State == FriendshipState.Accepted)
                {
                    throw new AccessViolationException("you are friend with this user");
                }
                else if (sentFriendship.State == FriendshipState.Rejected)
                {
                    throw new AccessViolationException("your previous Friendship rejected by this user");
                }
            }
            var receivedFriendship = await _repository.GetByFromUserAndToUserAsync(toUser.Id, fromUser.Id);
            if (receivedFriendship != null)
            {
                if (receivedFriendship.State == FriendshipState.Requested)
                {
                    throw new AccessViolationException("you have a Friendship from this user");
                }
                else if (receivedFriendship.State == FriendshipState.Accepted)
                {
                    throw new AccessViolationException("you are friend with this user");
                }
            }
            var friendship = new Friendship
            {
                FromUserId = fromUser.Id,
                ToUserId = toUser.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };
            var result = await _repository.InsertAsync(friendship);
            return result;
        }
        public async Task<IEnumerable<Friendship>> GetSentFriendshipsAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            return await _repository.GetByFromUserAndStateAsync(user.Id, FriendshipState.Requested);
        }
        public async Task<IEnumerable<Friendship>> GetReceivedFriendshipsAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            return await _repository.GetByToUserAndStateAsync(user.Id, FriendshipState.Requested);
        }
        public Task<int> AcceptFriendshipAsync(Guid id)
        {
            return ChangeStateAsync(id, FriendshipState.Accepted);
        }
        public Task<int> RejectFriendshipAsync(Guid id)
        {
            // doto: we can delete this Friendship after few days
            return ChangeStateAsync(id, FriendshipState.Rejected);
        }
        private async Task<int> ChangeStateAsync(Guid id, FriendshipState newState)
        {
            var user = await _userService.GetCurrentUserAsync();
            var friendship = await _repository.GetByIdAsync(id);
            if (friendship == null)
            {
                throw new EntryPointNotFoundException($"couldn't find any Friendship with id = {id}");
            }
            if (friendship.ToUserId != user.Id)
            {
                throw new AccessViolationException("you can't accept other users Friendship");
            }
            if (friendship.State != FriendshipState.Requested)
            {
                throw new InvalidOperationException("this Friendship state is not Requested");
            }
            friendship.State = newState;
            return await _repository.UpdateAsync(friendship);
        }
        public async Task<int> CancelFriendshipAsync(Guid id)
        {
            var user = await _userService.GetCurrentUserAsync();
            var friendship = await _repository.GetByIdAsync(id);
            if (friendship == null)
            {
                throw new EntryPointNotFoundException("couldn't find any Friendship with id = " + id);
            }
            if (friendship.FromUserId != user.Id)
            {
                throw new AccessViolationException("you can't cancel Friendship was sent by other users ");
            }
            if (friendship.State != FriendshipState.Requested)
            {
                throw new InvalidOperationException("this Friendship state is not Requested");
            }
            return await _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<User>> GetFriendsAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            var sentAcceptedships = await _repository.GetByFromUserAndStateAsync(user.Id, FriendshipState.Accepted);
            var receivedAcceptedships = await _repository.GetByToUserAndStateAsync(user.Id, FriendshipState.Accepted);
            List<User> friends = new List<User>();
            foreach (var item in sentAcceptedships)
            {
                friends.Add(item.ToUser);
            }
            foreach (var item in receivedAcceptedships)
            {
                friends.Add(item.FromUser);
            }
            return friends;
        }
        public async Task<int> UnfriendAsync(Guid userId)
        {
            var user = await _userService.GetCurrentUserAsync();
            var friendship = await _repository.GetByFromUserAndToUserAsync(user.Id, userId);
            if (friendship == null)
            {
                friendship = await _repository.GetByFromUserAndToUserAsync(userId, user.Id);
            }
            if (friendship == null)
            {
                throw new EntryPointNotFoundException("couldn't find any Friendship for userId = " + userId);
            }
            if (friendship.State != FriendshipState.Accepted)
            {
                throw new InvalidOperationException("your Friendship with this user state is not Accepted");
            }
            return await _repository.DeleteAsync(friendship.Id);
        }
    }
}
