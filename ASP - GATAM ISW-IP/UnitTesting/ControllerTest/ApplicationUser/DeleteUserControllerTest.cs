

using Gatam.Application.CQRS.User;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationUser;
[TestClass]
public class DeleteUserControllerTest
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
    public async Task DeleteUser_DeletesUser()
    {
        var user = new List<Gatam.Domain.ApplicationUser>
        {
            new Gatam.Domain.ApplicationUser
            {
                Username = "user1",
                Email = "user1@example.com",
                Id = "1"
            }
        };

        mediator.Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), default)).ReturnsAsync(true);
        var result = await controller.DeleteUser(user.First().Id);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        Assert.IsTrue(((OkObjectResult)result).StatusCode == 200);
    }

}