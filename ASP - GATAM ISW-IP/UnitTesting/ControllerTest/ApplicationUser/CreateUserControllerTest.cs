using Gatam.Application.CQRS;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
    public class CreateUserControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserController _userController;
        private IPasswordHasher<Gatam.Domain.ApplicationUser> _passwordHasher;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _userController = new UserController( _mediatorMock.Object );
            _passwordHasher = new PasswordHasher<Gatam.Domain.ApplicationUser>();
        }

        [TestMethod]
        public async Task CreateUser_ReturnsOk_AndUser()
        {
            Gatam.Domain.ApplicationUser user = new Gatam.Domain.ApplicationUser() { UserName="Test", Email="test@email.com" };
            user.PasswordHash = _passwordHasher.HashPassword(user, "test1234!@");

            _mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateUserCommand>(), default)).ReturnsAsync(user);

            IActionResult result = await _userController.CreateUser(user);

            OkObjectResult? okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(user, okObjectResult.Value);
        }
        [TestMethod]
        public async Task CreateUser_ReturnsError_EmptyMail()
        {
            Gatam.Domain.ApplicationUser user = new Gatam.Domain.ApplicationUser() { UserName = "Test", Email = "" };
            user.PasswordHash = _passwordHasher.HashPassword(user, "test1234!@");

            _mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateUserCommand>(), default)).ReturnsAsync(user);

            IActionResult result = await _userController.CreateUser(user);

            OkObjectResult? okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreNotEqual(200, okObjectResult.StatusCode);
            Assert.AreEqual(user, okObjectResult.Value);
        }
    }
}
