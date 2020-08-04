using Moq;
using NUnit.Framework;
using SocialNetworkWebApp.Exceptions;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Enums;
using SocialNetworkWebApp.Repositories.Contracts;
using SocialNetworkWebApp.Services;
using SocialNetworkWebApp.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.UnitTest
{
    public class FriendshipServiceUnitTest
    {

        private Mock<IUserService> mockUserService;
        private readonly User user1 = new User
        {
            Id = Guid.NewGuid(),
            UserName = "User1",
            FullName = "first User",
        };
        private readonly User user2 = new User
        {
            Id = Guid.NewGuid(),
            UserName = "User2",
            FullName = "second User",
        };
        private readonly User user3 = new User
        {
            Id = Guid.NewGuid(),
            UserName = "User3",
            FullName = "third User",
        };

        [OneTimeSetUp]
        public void Setup()
        {
            mockUserService = new Mock<IUserService>();
            mockUserService.Setup(service => service.GetCurrentUserAsync())
                .ReturnsAsync(user1);
            mockUserService.Setup(service => service.GetUserByIdAsync(user2.Id))
                .ReturnsAsync(user2);
            mockUserService.Setup(service => service.GetUserByIdAsync(user1.Id))
                .ReturnsAsync(user1);
        }

        #region CreateFriendshipAsync
        [Test]
        public async Task CreateFriendshipAsync_CreateNewFriendship_ReturnOne()
        {
            // Arrange
            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.InsertAsync(
                It.Is<Friendship>(x => x.FromUserId == user1.Id && x.ToUserId == user2.Id
                                    && x.State == FriendshipState.Requested)))
                .ReturnsAsync(1);
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.CreateFriendshipAsync(user2.Id);

            // Assert
            Assert.AreEqual(result, 1);
        }
        [Test]
        public async Task CreateFriendshipAsync_CreateFriendshipWithYourSelf_ThrowException()
        {
            // Arrange
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act and Assert
            Assert.ThrowsAsync(typeof(BadRequestExceptions), 
                () => friendshipService.CreateFriendshipAsync(user1.Id));

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CreateFriendshipAsync(user1.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you can't send Friendship request to yourself");
        }
        [Test]
        public async Task CreateFriendshipAsync_CreateExistedFriendship_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Requested,
            };
            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user1.Id, user2.Id))
                .ReturnsAsync(friendship);
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CreateFriendshipAsync(user2.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you have already sent a Friendship request to this user");
        }
        [Test]
        public async Task CreateFriendshipAsync_CreateFriendshipWithYourFriendRequestedByYou_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Accepted,
            };
            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user1.Id, user2.Id))
                .ReturnsAsync(friendship);
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CreateFriendshipAsync(user2.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you already are friend with this user");
        }
        [Test]
        public async Task CreateFriendshipAsync_CreateFriendshipWithYourFriendRequestedByYourFriend_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Accepted,
            };
            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user2.Id, user1.Id))
                .ReturnsAsync(friendship);
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CreateFriendshipAsync(user2.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you already are friend with this user");
        }
        [Test]
        public async Task CreateFriendshipAsync_CreateFriendshipWithSomeoneWhoRejectedYouRecently_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Rejected,
            };
            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user1.Id, user2.Id))
                .ReturnsAsync(friendship);
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CreateFriendshipAsync(user2.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "your previous Friendship request rejected by this user");
        }
        #endregion

        #region GetSentFriendshipsAsync
        [Test]
        public async Task GetSentFriendshipsAsync_HaveSentRequestedFriendship_ReturnListOfFriendship()
        {
            // Arrange
            var friendship1 = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };
            var friendship2 = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user3.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndStateAsync(user1.Id, FriendshipState.Requested))
                .ReturnsAsync(new List<Friendship> { friendship1, friendship2 });

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.GetSentFriendshipsAsync();

            // Assert
            Assert.AreEqual(result.Count(), 2);
        }
        [Test]
        public async Task GetSentFriendshipsAsync_HaveNotSentRequestedFriendship_ReturnEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.GetSentFriendshipsAsync();

            // Assert
            Assert.IsEmpty(result);
        }
        #endregion

        #region GetReceivedFriendshipsAsync
        [Test]
        public async Task GetReceivedFriendshipsAsync_HaveReceivedRequestedFriendship_ReturnListOfFriendship()
        {
            // Arrange
            var friendship1 = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };
            var friendship2 = new Friendship
            {
                FromUserId = user3.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByToUserAndStateAsync(user1.Id, FriendshipState.Requested))
                .ReturnsAsync(new List<Friendship> { friendship1, friendship2 });

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.GetReceivedFriendshipsAsync();

            // Assert
            Assert.AreEqual(result.Count(), 2);
        }
        [Test]
        public async Task GetReceivedFriendshipsAsync_HaveNotReceivedRequestedFriendship_ReturnEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.GetReceivedFriendshipsAsync();

            // Assert
            Assert.IsEmpty(result);
        }
        #endregion

        #region AcceptFriendshipAsync
        [Test]
        public async Task AcceptFriendshipAsync_AcceptYourFriendshipRequest_ReturnOne()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);
            mockRepository.Setup(repository => repository.UpdateAsync(
                It.Is<Friendship>(x => x.FromUserId == user2.Id && x.ToUserId == user1.Id
                                    && x.Id == friendship.Id && x.State == FriendshipState.Accepted)))
                .ReturnsAsync(1);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.AcceptFriendshipAsync(friendship.Id);

            // Assert
            Assert.AreEqual(result, 1);
        }
        [Test]
        public async Task AcceptFriendshipAsync_InvalidFriendshipId_ThrowException()
        {
            // Arrange
            var friendshipId = Guid.NewGuid();
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.AcceptFriendshipAsync(friendshipId);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, $"couldn't find any Friendship with id = {friendshipId}");
        }
        [Test]
        public async Task AcceptFriendshipAsync_AcceptOtherUserFriendshipRequest_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.AcceptFriendshipAsync(friendship.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you can't accept other users Friendship request");
        }
        [Test]
        public async Task AcceptFriendshipAsync_FriendshipStateNotRequested_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.AcceptFriendshipAsync(friendship.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "this Friendship state is not Requested");
        }
        #endregion

        #region RejectFriendshipAsync
        [Test]
        public async Task RejectFriendshipAsync_RejectYourFriendshipRequest_ReturnOne()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);
            mockRepository.Setup(repository => repository.UpdateAsync(
                It.Is<Friendship>(x => x.FromUserId == user2.Id && x.ToUserId == user1.Id
                                    && x.Id == friendship.Id && x.State == FriendshipState.Rejected)))
                .ReturnsAsync(1);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.RejectFriendshipAsync(friendship.Id);

            // Assert
            Assert.AreEqual(result, 1);
        }
        [Test]
        public async Task RejectFriendshipAsync_InvalidFriendshipId_ThrowException()
        {
            // Arrange
            var friendshipId = Guid.NewGuid();
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.RejectFriendshipAsync(friendshipId);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, $"couldn't find any Friendship with id = {friendshipId}");
        }
        [Test]
        public async Task RejectFriendshipAsync_RejectOtherUserFriendshipRequest_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.RejectFriendshipAsync(friendship.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you can't accept other users Friendship request");
        }
        [Test]
        public async Task RejectFriendshipAsync_FriendshipStateNotRequested_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.RejectFriendshipAsync(friendship.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "this Friendship state is not Requested");
        }
        #endregion

        #region CancelFriendshipAsync
        [Test]
        public async Task CancelFriendshipAsync_CancelYourFriendshipRequest_ReturnOne()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);
            mockRepository.Setup(repository => repository.DeleteAsync(friendship.Id))
                .ReturnsAsync(1);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.CancelFriendshipAsync(friendship.Id);

            // Assert
            Assert.AreEqual(result, 1);
        }
        [Test]
        public async Task CancelFriendshipAsync_InvalidFriendshipId_ThrowException()
        {
            // Arrange
            var friendshipId = Guid.NewGuid();
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CancelFriendshipAsync(friendshipId);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, $"couldn't find any Friendship with id = {friendshipId}");
        }
        [Test]
        public async Task CancelFriendshipAsync_CancelOtherUserFriendshipRequest_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CancelFriendshipAsync(friendship.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "you can't cancel Friendship was sent by other users");
        }
        [Test]
        public async Task CancelFriendshipAsync_FriendshipStateNotRequested_ThrowException()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByIdAsync(friendship.Id))
                .ReturnsAsync(friendship);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.CancelFriendshipAsync(friendship.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "this Friendship state is not Requested");
        }
        #endregion

        #region GetFriendsAsync
        [Test]
        public async Task GetFriendsAsync_HaveFriends_ReturnListOfFriends()
        {
            // Arrange
            var friendship1 = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };
            var friendship2 = new Friendship
            {
                FromUserId = user3.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndStateAsync(user1.Id, FriendshipState.Accepted))
                .ReturnsAsync(new List<Friendship> { friendship1 });
            mockRepository.Setup(repository => repository.GetByToUserAndStateAsync(user1.Id, FriendshipState.Accepted))
                .ReturnsAsync(new List<Friendship> { friendship2 });

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.GetFriendsAsync();

            // Assert
            Assert.AreEqual(result.Count(), 2);
        }
        [Test]
        public async Task GetFriendsAsync_HaveNotFriendship_ReturnEmpty()
        {
            // Arrange
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.GetFriendsAsync();

            // Assert
            Assert.IsEmpty(result);
        }
        #endregion

        #region UnfriendAsync
        [Test]
        public async Task UnfriendAsync_UnfriendUserRequestedByYou_ReturnOne()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user1.Id,
                ToUserId = user2.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user1.Id, user2.Id))
                .ReturnsAsync(friendship);

            mockRepository.Setup(repository => repository.DeleteAsync(friendship.Id))
                .ReturnsAsync(1);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.UnfriendAsync(user2.Id);

            // Assert
            Assert.AreEqual(result, 1);
        }
        [Test]
        public async Task UnfriendAsync_UnfriendUserRequestedByHe_ReturnOne()
        {
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };

            var mockRepository = new Mock<IFriendshipRepository>();
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user2.Id, user1.Id))
                .ReturnsAsync(friendship);

            mockRepository.Setup(repository => repository.DeleteAsync(friendship.Id))
                .ReturnsAsync(1);

            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            var result = await friendshipService.UnfriendAsync(user2.Id);

            // Assert
            Assert.AreEqual(result, 1);
        }
        [Test]
        public async Task UnfriendAsync_ThereIsNoFriendship_ThrowException()
        {
            // Arrange
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.UnfriendAsync(user2.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, $"couldn't find any Friendship between current user and userId = {user2.Id}");
        }
        [Test]
        public async Task UnfriendAsync_FriendshipStateIsNotAcepted_ThrowException()
        {
            // Arrange
            // Arrange
            var friendship = new Friendship
            {
                FromUserId = user2.Id,
                ToUserId = user1.Id,
                State = FriendshipState.Requested,
                Id = Guid.NewGuid(),
            };
            var mockRepository = new Mock<IFriendshipRepository>();
            var friendshipService = new FriendshipService(mockRepository.Object, mockUserService.Object);
            mockRepository.Setup(repository => repository.GetByFromUserAndToUserAsync(user2.Id, user1.Id))
                .ReturnsAsync(friendship);

            // Act
            Exception expectedExcetpion = null;
            try
            {
                var result = await friendshipService.UnfriendAsync(user2.Id);
            }
            catch (Exception ex)
            {
                expectedExcetpion = ex;
            }

            // Assert
            Assert.IsNotNull(expectedExcetpion);
            Assert.IsInstanceOf<BadRequestExceptions>(expectedExcetpion);
            Assert.AreEqual(expectedExcetpion.Message, "your Friendship with this user state is not Accepted");
        }
        #endregion
    }
}
