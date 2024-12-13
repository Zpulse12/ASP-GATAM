using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.Application.CQRS.DTOS.UsersDTO;


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
        //[TestMethod]
        //public async Task CreateUser_ReturnsCreated_AndUser()
        //{
        //    var user = new CreateUserDTO
        //    {
        //        Id = "12345",
        //        Name = "John",
        //        Username = "john_doe",
        //        Surname = "Smith",
        //        Email = "john.smith@example.com",
        //        PhoneNumber = "+123456789012",
        //        Picture = "https://example.com/profile.jpg",
        //        MentorId = "begeleider123",
        //        IsActive = true,
        //    };

        //    //_mediatorMock.Setup(setup => setup.Send(It.IsAny<CreateAuth0UserCommand>(), default)).ReturnsAsync(user);

        //    IActionResult result = await _userController.CreateUser(user);

        //    CreatedResult? createdResult = result as CreatedResult;
        //    Assert.IsNotNull(createdResult);
        //    Assert.AreEqual(201, createdResult.StatusCode);
        //    Assert.AreEqual(user, createdResult.Value);
        //}
        //[TestMethod]
        //public async Task CreateUser_ReturnsBadRequest_EmptyMail()
        //{
        //    _userController.ModelState.AddModelError("Email", "Email cannot be empty");
        //    CreateUserDTO user = new CreateUserDTO { Name = "Test", Email = "" };
        //    IActionResult result = await _userController.CreateUser(user);
        //    Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        //}
    }
}
