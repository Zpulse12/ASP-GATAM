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
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace UnitTesting.ControllerTest.ApplicationUser
{
    [TestClass]
    public class DeactivateUserControllerTest
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
        public async Task DeactivateUSer_Test()
        {

            var userId = "123";
            var command = new DeactivateUserCommand { _userId = userId, IsActive = false };
            var user = new List<UserDTO>
            {
                new UserDTO
                {
                    Username = "user1",
                    Email = "user1@example.com",
                    Id = "1"
                }
            };

            //mediator.Setup(m => m.Send(It.IsAny<DeactivateUserCommand>(), default)).ReturnsAsync(user);

            var result = await controller.SetActiveState(userId, command);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(user, okResult.Value);

        }

    }
}
