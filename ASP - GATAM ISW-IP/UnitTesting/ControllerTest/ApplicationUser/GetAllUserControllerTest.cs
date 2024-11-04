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
                    Username = "user1",
                    Email = "user1@example.com",
                    Id = "1"
                },
                new UserDTO
                {
                    Username = "user2",
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
            Assert.AreEqual(users, okResult.Value);
        }
    }
}
