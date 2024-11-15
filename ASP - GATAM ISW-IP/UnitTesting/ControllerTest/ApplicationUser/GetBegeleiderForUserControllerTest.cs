using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationUser;
[TestClass]
public class GetBegeleiderForUserControllerTest
{
    private Mock<IMediator> _mockMediator;
    private UserController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockMediator = new Mock<IMediator>();
        _controller = new UserController(_mockMediator.Object);
    }

    [TestMethod]
    public async Task GetBegeleiderForUser_ShouldReturnOk_WhenBegeleiderExists()
    {
        var userDto = new UserDTO
        {
            Id = "begeleider123",
            Username = "BegeleiderNaam",
            Email = null,
            RolesIds = null
        };
        _mockMediator.Setup(m => m.Send(It.IsAny<GetBegeleiderForUserQuery>(), default))
            .ReturnsAsync(userDto);
        var result = await _controller.GetBegeleiderForUser("user123") as OkObjectResult;
        Assert.IsNotNull(result);
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsInstanceOfType(result.Value, typeof(UserDTO));
    }
}