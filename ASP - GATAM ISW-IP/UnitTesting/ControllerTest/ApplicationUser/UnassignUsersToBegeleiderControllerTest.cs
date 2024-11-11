using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Application.CQRS.User;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

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

        [TestMethod]
        public async Task UnassignUsersToBegeleider_UserExists_ReturnsOkResult()
        {
            var userId = "volgerId";
            var expectedResponse = true; // Verwacht een boolean resultaat (true)
            var begeleiderId = "begeleiderId";

            _mediatorMock.Setup(m => m.Send(It.IsAny<FindUserByIdQuery>(), default))
                         .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = userId });

            _mediatorMock.Setup(m => m.Send(It.IsAny<Gatam.Application.CQRS.User.BegeleiderAssignment.AssignUserToBegeleiderCommand>(), default))
                      .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId", BegeleiderId = begeleiderId });

            _mediatorMock.Setup(m => m.Send(It.IsAny<UnassignUserCommand>(), default))
                  .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId" });

            // Act
            var result = await _controller.UnassignUsersToBegeleider(userId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }


        [TestMethod]
        public async Task UnassignUsersToBegeleider_UserDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var userId = "nonExistentUserId";
            _mediatorMock.Setup(m => m.Send(It.IsAny<FindUserByIdQuery>(), default))
                .ReturnsAsync((Gatam.Domain.ApplicationUser)null);

            // Act
            var result = await _controller.UnassignUsersToBegeleider(userId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

    }
}
