using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.Application.Extensions;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationUser;
[TestClass]
public class UpdateUserControllerTest
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
    public async Task UpdateUser_ShouldReturnOk_WhenUserIsUpdatedSuccessfully()
    { // Arrange
        var userDto = new UserDTO
        {
            Id = "1234",
            Nickname = "TestUser",
            Email = "testuser@example.com",
            RolesIds = new List<string> { RoleMapper.Roles["BEHEERDER"] },
            IsActive = true
        };

        mediator.Setup(m => m.Send(It.Is<UpdateUserCommand>(cmd => cmd.Id == userDto.Id && cmd.User == userDto), default))
            .ReturnsAsync(userDto);
        // Act
        var result = await controller.UpdateUser(userDto.Id, userDto);
        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(userDto, okResult.Value);
    }


}