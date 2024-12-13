using Gatam.Application.CQRS.Questions;
using Gatam.Domain;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.Question
{
    [TestClass]
    public class UpdateVisibilityControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private QuestionController _questionController;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _questionController = new QuestionController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task UpdateVisibility_Returns_Success()
        {
            string userQuestionId = "test-id";
            bool isVisible = true;
            var expectedUserQuestion = new UserQuestion { Id = userQuestionId, IsVisible = isVisible };
            
            _mediatorMock.Setup(m => m.Send(It.Is<UpdateQuestionVisibilityCommand>(c => 
                c.UserQuestionId == userQuestionId && 
                c.IsVisible == isVisible), 
                default))
                .ReturnsAsync(expectedUserQuestion);

            var result = await _questionController.UpdateVisibility(userQuestionId, isVisible);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedUserQuestion, okResult.Value);
        }

        [TestMethod]
        public async Task UpdateVisibility_SendsCorrectCommand()
        {
            string userQuestionId = "test-id";
            bool isVisible = true;
            UpdateQuestionVisibilityCommand capturedCommand = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuestionVisibilityCommand>(), default))
                .Callback<IRequest<UserQuestion>, CancellationToken>((cmd, _) => 
                    capturedCommand = (UpdateQuestionVisibilityCommand)cmd)
                .ReturnsAsync(new UserQuestion());

            await _questionController.UpdateVisibility(userQuestionId, isVisible);

            Assert.IsNotNull(capturedCommand);
            Assert.AreEqual(userQuestionId, capturedCommand.UserQuestionId);
            Assert.AreEqual(isVisible, capturedCommand.IsVisible);
        }
    }
} 