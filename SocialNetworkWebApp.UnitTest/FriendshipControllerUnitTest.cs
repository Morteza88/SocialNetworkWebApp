using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SocialNetworkWebApp.Controllers;
using SocialNetworkWebApp.Models.DBModels;
using SocialNetworkWebApp.Models.Dtos;
using SocialNetworkWebApp.Models.Enums;
using SocialNetworkWebApp.Services.Contracts;
using SocialNetworkWebApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkWebApp.UnitTest
{
    public class FriendshipControllerUnitTest
    {
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
        private IMapper mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            mapper = mappingConfig.CreateMapper();
        }

        #region GetSentFriendShips
        [Test]
        public async Task GetSentFriendShips_HaveSentFriendships_ReturnOkObjectResultWithListOfSentFriendsipDtos()
        {
            // Arrange
            var friendship1 = new Friendship
            {
                FromUserId = user1.Id,
                FromUser = user1,
                ToUserId = user2.Id,
                ToUser = user2,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };
            var friendship2 = new Friendship
            {
                FromUserId = user1.Id,
                FromUser = user1,
                ToUserId = user3.Id,
                ToUser = user3,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };
            var mockService = new Mock<IFriendshipService>();
            mockService.Setup(service => service.GetSentFriendshipsAsync())
                .ReturnsAsync(new List<Friendship> { friendship1, friendship2 });

            var controller = new FriendshipController(mockService.Object, mapper);

            // Act
            var result = await controller.GetSentFriendShips();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result.Result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.AreEqual(objectResult.StatusCode, 200);
            var sentFriendshipDtos = (IEnumerable<SentFriendshipDto>)(result.Result as OkObjectResult).Value;
            Assert.AreEqual(sentFriendshipDtos.Count(), 2);
        }
        [Test]
        public async Task GetSentFriendShips_HaveNotSentFriendships_ReturnOkObjectResultWithEmptyList()
        {
            // Arrange
            var mockService = new Mock<IFriendshipService>();

            var controller = new FriendshipController(mockService.Object, mapper);

            // Act
            var result = await controller.GetSentFriendShips();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result.Result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.AreEqual(objectResult.StatusCode, 200);
            var sentFriendshipDtos = (IEnumerable<SentFriendshipDto>)(result.Result as OkObjectResult).Value;
            Assert.IsEmpty(sentFriendshipDtos);
        }
        #endregion

        #region GetReceivedFriendships
        [Test]
        public async Task GetReceivedFriendships_HaveReceivedFriendships_ReturnOkObjectResultWithListOfReceivedFriendsipDtos()
        {
            // Arrange
            var friendship1 = new Friendship
            {
                FromUserId = user2.Id,
                FromUser = user2,
                ToUserId = user1.Id,
                ToUser = user1,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };
            var friendship2 = new Friendship
            {
                FromUserId = user3.Id,
                FromUser = user3,
                ToUserId = user1.Id,
                ToUser = user1,
                State = FriendshipState.Accepted,
                Id = Guid.NewGuid(),
            };
            var mockService = new Mock<IFriendshipService>();
            mockService.Setup(service => service.GetReceivedFriendshipsAsync())
                .ReturnsAsync(new List<Friendship> { friendship1, friendship2 });

            var controller = new FriendshipController(mockService.Object,mapper);

            // Act
            var result = await controller.GetReceivedFriendships();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result.Result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.AreEqual(objectResult.StatusCode, 200);
            var sentFriendshipDtos = (IEnumerable<ReceivedFriendshipDto>)(result.Result as OkObjectResult).Value;
            Assert.AreEqual(sentFriendshipDtos.Count(), 2);
        }
        [Test]
        public async Task GetReceivedFriendships_HaveNotReceivedFriendships_ReturnOkObjectResultWithEmptyList()
        {
            // Arrange
            var mockService = new Mock<IFriendshipService>();

            var controller = new FriendshipController(mockService.Object,mapper);

            // Act
            var result = await controller.GetReceivedFriendships();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ActionResult>(result.Result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var objectResult = (result.Result as ObjectResult);
            Assert.AreEqual(objectResult.StatusCode, 200);
            var sentFriendshipDtos = (IEnumerable<ReceivedFriendshipDto>)(result.Result as OkObjectResult).Value;
            Assert.IsEmpty(sentFriendshipDtos);
        }
        #endregion
    }
}
