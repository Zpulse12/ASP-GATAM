using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;


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
            _userController = new UserController(_mediatorMock.Object);
            _passwordHasher = new PasswordHasher<Gatam.Domain.ApplicationUser>();
        }
        [TestMethod]
        public async Task CreateUser_ReturnsCreated_AndUser()
        {
            UserDTO user = new UserDTO() { Name="Test", Email="test@email.com" };

            _mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateUserCommand>(), default)).ReturnsAsync(user);

            IActionResult result = await _userController.CreateUser(user);

            CreatedResult? createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(user, createdResult.Value);
        }
        //[TestMethod]
        //public async Task CreateUser_ReturnsBadRequest_EmptyMail()
        //{
        //    _userController.ModelState.AddModelError("Email", "Email cannot be empty");
        //    UserDTO user = new UserDTO { Name = "Test", Email = "" };
        //    IActionResult result = await _userController.CreateUser(user);
        //    Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        //}
    }
}
