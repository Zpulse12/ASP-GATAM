using FluentValidation;
using Gatam.Application.Behaviours;
using Gatam.Application.CQRS.Questions;
using Gatam.Domain;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.Question
{
    [TestClass]
    public class UpdateQuestionControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private QuestionController _questionController;
        private IValidator<UpdateQuestionCommand> _validator;
        private ValidationBehaviour<UpdateQuestionCommand, Gatam.Domain.Question> _validationBehavior;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _questionController = new QuestionController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task UpdateQuestionReturns_UpdatedQuestion()
        {
            // Arrange
            string questionId = "1";
            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } };
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Updated Title?", QuestionType = (short)QuestionType.OPEN, Answers = answerList };
            _mediatorMock.Setup(setup => setup.Send(It.IsAny<UpdateQuestionCommand>(), default)).ReturnsAsync(question);

            // Act
            IActionResult result = await _questionController.UpdateQuestion(questionId, question);

            // Assert
            OkObjectResult okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(question, okResult.Value);
        }

        [TestMethod]
        public async Task UpdateQuestion_failsOn_emptyTitle()
        {
            // Arrange
            string questionId = "1";
            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } };
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "", QuestionType = (short)QuestionType.OPEN, Answers = answerList };
            UpdateQuestionCommand invalidCommand = new UpdateQuestionCommand() { Question = question, Id = questionId };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
                await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
        }

        [TestMethod]
        public async Task UpdateQuestion_failsOn_missingQuestionType()
        {
            // Arrange
            string questionId = "1";
            List<QuestionAnswer> answerList = new List<QuestionAnswer>() { new QuestionAnswer() { Answer = "OPEN", AnswerValue = "OPEN" } };
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Updated Title?", Answers = answerList };
            UpdateQuestionCommand invalidCommand = new UpdateQuestionCommand() { Question = question, Id = questionId };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
                await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
        }

        [TestMethod]
        public async Task UpdateQuestion_failsOn_emptyAnswerList()
        {
            // Arrange
            string questionId = "1";
            Gatam.Domain.Question question = new Gatam.Domain.Question() { QuestionTitle = "Updated Title?", QuestionType = (short)QuestionType.OPEN };
            UpdateQuestionCommand invalidCommand = new UpdateQuestionCommand() { Question = question, Id = questionId };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
                await _validationBehavior.Handle(invalidCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None));
        }

    }
}
