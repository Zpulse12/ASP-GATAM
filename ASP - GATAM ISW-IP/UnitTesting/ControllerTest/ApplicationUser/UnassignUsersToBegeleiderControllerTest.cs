using Gatam.WebAPI.Controllers;
using MediatR;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationUser
{
    [TestClass]
    public class UnassignUsersToBegeleiderControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        // [TestMethod]
        // public async Task UnassignUsersToBegeleider_UserExists_ReturnsOkResult()
        // {
        //     var userId = new Gatam.Domain.ApplicationUser() { Id = "volgerId" };
        //     var expectedResponse = true; // Verwacht een boolean resultaat (true)
        //     var begeleiderId = "begeleiderId";
        //
        //     _mediatorMock.Setup(m => m.Send(It.IsAny<FindUserByIdQuery>(), default))
        //                  .ReturnsAsync(new UserDTO() { Id = "userId" });
        //
        //     _mediatorMock.Setup(m => m.Send(It.IsAny<Gatam.Application.CQRS.User.BegeleiderAssignment.AssignUserToBegeleiderCommand>(), default))
        //               .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId", BegeleiderId = begeleiderId });
        //
        //     _mediatorMock.Setup(m => m.Send(It.IsAny<UnassignUserCommand>(), default))
        //           .ReturnsAsync(new UserDTO { Id = "volgerId" });
        //
        //     // Act
        //     var result = await _controller.UnassignUsersToBegeleider(userId);
        //
        //     var okResult = result as OkObjectResult;
        //     Assert.IsNotNull(okResult);
        //     Assert.AreEqual(200, okResult.StatusCode);
        // }

    }
}
