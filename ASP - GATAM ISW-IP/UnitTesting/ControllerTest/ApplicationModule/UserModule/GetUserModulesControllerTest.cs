using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Domain;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationModule.UserModule
{
    [TestClass]
    public class GetUserModulesControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserModuleController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserModuleController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task SubmitAnswers_ReturnsOkResult_WithUpdatedUserAnswers()
        {
            var userModuleId = "module1";
            var userAnswers = new List<UserAnswer>
    {
        new UserAnswer { Id = "answer1", GivenAnswer = "TestAnswer" }
    };
            var updatedAnswers = userAnswers;

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateUserModuleStatusCommand>(), default))
                .ReturnsAsync(new Gatam.Domain.UserModule
                {
                    Id = userModuleId,
                    State = UserModuleState.InProgress
                });

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SubmitUserAnswersCommand>(), default))
                .ReturnsAsync(updatedAnswers);

            var result = await _controller.SubmitAnswers(userModuleId, userAnswers);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedAnswers = okResult.Value as List<UserAnswer>;
            Assert.IsNotNull(returnedAnswers);
            Assert.AreEqual(userAnswers.Count, returnedAnswers.Count);

            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateUserModuleStatusCommand>(), default), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SubmitUserAnswersCommand>(), default), Times.Once);
        }

    }
}
