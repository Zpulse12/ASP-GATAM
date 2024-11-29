using Gatam.Application.CQRS.Questions.Gatam.Application.CQRS.Questions;
using Gatam.Domain;
using Gatam.WebAPI.Controllers;
using Gatam.WebAppBegeleider.Extensions.RequestObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.Question
{
    [TestClass]
    public class UpdateQuestionPriorityControllerTest
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
            QuestionPriority priority = QuestionPriority.MEDIUM;
            var expectedUserQuestion = new UserQuestion { Id = userQuestionId, QuestionPriority = priority };

            _mediatorMock.Setup(m => m.Send(It.Is<UpdateQuestionPriorityCommand>(c =>
                c.UserQuestionId == userQuestionId &&
                c.Priority == priority),
                default))
                .ReturnsAsync(expectedUserQuestion);

            var result = await _questionController.UpdateUserQuestionPriority(userQuestionId, new UpdateQuestionPriorityRequestObject() { QuestionPriority = priority});

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            Assert.AreEqual(expectedUserQuestion, okResult.Value);
        }

        [TestMethod]
        public async Task UpdateVisibility_SendsCorrectCommand()
        {
            string userQuestionId = "test-id";
            QuestionPriority priority = QuestionPriority.MEDIUM;
            UpdateQuestionPriorityCommand capturedCommand = null;

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateQuestionPriorityCommand>(), default))
                .Callback<IRequest<UserQuestion>, CancellationToken>((cmd, _) =>
                    capturedCommand = (UpdateQuestionPriorityCommand)cmd)
                .ReturnsAsync(new UserQuestion());

            await _questionController.UpdateUserQuestionPriority(userQuestionId, new UpdateQuestionPriorityRequestObject() { QuestionPriority = priority });

            Assert.IsNotNull(capturedCommand);
            Assert.AreEqual(userQuestionId, capturedCommand.UserQuestionId);
            Assert.AreEqual(priority, capturedCommand.Priority);
        }
    }
}
