using Gatam.Application.CQRS;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task CreateUser_ReturnsCreated_AndUser()
        {
            Gatam.Domain.ApplicationUser user = new Gatam.Domain.ApplicationUser() { UserName="Test", Email="test@email.com" };
            user.PasswordHash = _passwordHasher.HashPassword(user, "test1234!@");

            _mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateUserCommand>(), default)).ReturnsAsync(user);

            IActionResult result = await _userController.CreateUser(user);

            CreatedResult? createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(user, createdResult.Value);
        }
        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_EmptyMail()
        {
            _userController.ModelState.AddModelError("Email", "Email cannot be empty");
            Gatam.Domain.ApplicationUser user = new Gatam.Domain.ApplicationUser() { UserName = "Test", Email = "" };
            user.PasswordHash = _passwordHasher.HashPassword(user, "test1234!@");


            _mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateUserCommand>(), default)).ReturnsAsync(user);

            IActionResult result = await _userController.CreateUser(user);
            Debug.WriteLine(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }
    }
}
