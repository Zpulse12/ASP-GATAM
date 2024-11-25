using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Domain;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.ApplicationModule.UserModule
{
    [TestClass]
    public class GetUserModuleByIdControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserModuleController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
           

            _controller = new UserModuleController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task GetUserModules_ReturnsOkResult_WithModules()
        {
            // Arrange
            var userId = "user1";
            var modules = new List<UserModuleDTO>
            {
                new UserModuleDTO { Id = "module1" },
                new UserModuleDTO { Id = "module2" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserModuleByUserQuery>(), default))
                .ReturnsAsync(modules);

            // Act
            var result = await _controller.GetUserModules(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedModules = okResult.Value as List<UserModuleDTO>;
            Assert.IsNotNull(returnedModules);
            Assert.AreEqual(modules.Count, returnedModules.Count);
            Assert.AreEqual(modules[0].Id, returnedModules[0].Id);
            Assert.AreEqual(modules[1].Id, returnedModules[1].Id);

            // Verify that the correct query was sent
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetUserModuleByUserQuery>(), default), Times.Once);
        }

    }
}
