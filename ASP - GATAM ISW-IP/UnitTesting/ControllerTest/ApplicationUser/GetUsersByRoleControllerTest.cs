using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User.Roles;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.ApplicationUser
{
    [TestClass]
    public class GetUsersByRoleControllerTests
    {
        private Mock<IMediator> _mediatorMock;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task GetUsersByRole_ValidRoleId_ReturnsOkWithUsers()
        {
            // Arrange
            string roleId = "rol_2SgoYL1AoK9tXYXW";
            var mockUsers = new List<UserDTO>
        {
            new UserDTO { Id = "1", Name = "User1", Email = "user1@example.com" },
            new UserDTO { Id = "2", Name = "User2", Email = "user2@example.com" }
        };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetUsersByRoleQuery>(q => q.RoleId == roleId), default))
                .ReturnsAsync(mockUsers);

            // Act
            var result = await _controller.GetUsersByRole(roleId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedUsers = okResult.Value as List<UserDTO>;
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(2, returnedUsers.Count);
        }

        [TestMethod]
        public async Task GetUsersByRole_InvalidRoleId_ReturnsOkWithEmptyList()
        {
            // Arrange
            string roleId = "invalid_role_id";
            var mockUsers = new List<UserDTO>();

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetUsersByRoleQuery>(q => q.RoleId == roleId), default))
                .ReturnsAsync(mockUsers);

            // Act
            var result = await _controller.GetUsersByRole(roleId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var returnedUsers = okResult.Value as List<UserDTO>;
            Assert.IsNotNull(returnedUsers);
            Assert.AreEqual(0, returnedUsers.Count);
        }

        
    }
}
