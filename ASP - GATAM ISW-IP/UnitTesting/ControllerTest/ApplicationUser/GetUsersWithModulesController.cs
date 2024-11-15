using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationUser;
[TestClass]
public class GetUsersWithModulesController
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
    public async Task GetUsersWithModules_ShouldReturnOk_WhenUsersExist()
    {
        var users = new List<UserDTO>
        {
            new UserDTO
            {
                Id = "2",
                Username = "User2",
                Email = "user2@example.com",
                Modules = new List<string>
                {
                    "Module2"
                },
                RolesIds = null
            }
        };
        users.Add(new UserDTO
        {
            Id = "1",
            Username = "User1",
            Email = "user1@example.com",
            Modules = new List<string>
            {
                "Module1"
            },
            RolesIds = null
        });

        mediator.Setup(m => m.Send(It.IsAny<GetUsersWithModulesQuery>(), default))
            .ReturnsAsync(users);

        var result = await controller.GetUsersWithModules();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(users, okResult.Value);
    }

    [TestMethod]
    public async Task GetUsersWithModules_ShouldReturnOk_WhenNoUsersExist()
    {
        mediator.Setup(m => m.Send(It.IsAny<GetUsersWithModulesQuery>(), default))
            .ReturnsAsync(new List<UserDTO>());

        var result = await controller.GetUsersWithModules();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(0, (okResult.Value as List<UserDTO>).Count);
    }
}

