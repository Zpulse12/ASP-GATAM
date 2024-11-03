using Gatam.Application.CQRS;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Application.CQRS.User;

namespace UnitTesting.ControllerTest.ApplicationUser
{
    [TestClass]
    public class GetAllTeamControllerTest
    {
        private Mock<IMediator>? mediator;
        private UserController? controller;

        [TestInitialize]
        public void Setup()
        {
            mediator = new Mock<IMediator>();
            controller = new UserController(mediator.Object);
        }

        [TestMethod]
        public async Task GetUsers_ReturnsOkResult_WithUsers()
        {
            var users = new List<UserDTO>
            {
                new UserDTO
                {
                    Nickname = "user1",
                    Email = "user1@example.com",
                    Id = "1"
                },
                new UserDTO
                {
                    Nickname = "user2",
                    Email = "user2@example.com",
                    Id = "2"
                }
            };

            mediator.Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), default))
                          .ReturnsAsync(users);

            var result = await controller.GetUsers();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var actualUsers = okResult.Value as List<UserDTO>;
            Assert.IsNotNull(actualUsers);
            Assert.AreEqual(users.Count, actualUsers.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.AreEqual(users[i].Nickname, actualUsers[i].Nickname, $"Mismatch at index {i} for Nickname");
                Assert.AreEqual(users[i].Email, actualUsers[i].Email, $"Mismatch at index {i} for Email");
                Assert.AreEqual(users[i].Id, actualUsers[i].Id, $"Mismatch at index {i} for Id");
            }
        }
    }
}
