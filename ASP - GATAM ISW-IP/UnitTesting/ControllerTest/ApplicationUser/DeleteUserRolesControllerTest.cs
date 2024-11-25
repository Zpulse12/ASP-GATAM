using Gatam.Application.CQRS.DTOS.RolesDTO;
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
    public class DeleteUserRolesControllerTest
    {
        private Mock<IMediator>? _mediator;
        private UserController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _controller = new UserController(_mediator.Object);
        }

        [TestMethod]
        public async Task DeleteUserRoles_DeletesRolesSuccessfully()
        {
            var userId = "1";
            var rolesToDelete = new List<string> { "Admin", "User" };
            var command = new DeleteUserRolesCommand
            {
                UserId = userId,
                Roles = new RolesDTO() { Roles = rolesToDelete}
            };
            _mediator.Setup(m => m.Send(It.IsAny<DeleteUserRolesCommand>(), default))
                    .ReturnsAsync(new Gatam.Application.CQRS.UserDTO());
            var result = await _controller.RemoveUserRoles(command.Roles, userId);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(((OkObjectResult)result).StatusCode == 200);
        }
    }
}
