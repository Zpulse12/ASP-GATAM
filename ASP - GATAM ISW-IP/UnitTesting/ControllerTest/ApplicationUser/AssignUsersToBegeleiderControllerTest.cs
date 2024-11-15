using Gatam.Application.CQRS.User;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationUser
{
    [TestClass]
    public class AssignUsersToBegeleiderControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task AssignUsersToBegeleider_UserExists_ReturnsOkResult()
        {
            var user = new Gatam.Domain.ApplicationUser { Id = "volgerId" };
            var begeleiderId = "begeleiderId";

            _mediatorMock.Setup(m => m.Send(It.IsAny<FindUserByIdQuery>(), default))
                         .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId" });

            _mediatorMock.Setup(m => m.Send(It.IsAny<Gatam.Application.CQRS.User.BegeleiderAssignment.AssignUserToBegeleiderCommand>(), default))
                         .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId", BegeleiderId = begeleiderId });

            var result = await _controller.AssignUsersToBegeleider(user, begeleiderId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}
