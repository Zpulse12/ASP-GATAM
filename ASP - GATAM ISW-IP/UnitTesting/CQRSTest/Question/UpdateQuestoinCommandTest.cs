using FluentValidation;
using Gatam.Application.Behaviours;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.Exceptions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.CQRSTest.Question
{
    [TestClass]
    public class UpdateQuestoinCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private UpdateQuestionCommandHandler _handler;
        private ValidationBehaviour<UpdateQuestionCommand, Gatam.Domain.Question> _commandBehaviour;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _handler = new UpdateQuestionCommandHandler(_unitOfWork.Object); 
            UpdateQuestionCommandValidator validator = new UpdateQuestionCommandValidator(_unitOfWork.Object);
            IEnumerable<IValidator<UpdateQuestionCommand>> validators = new List<IValidator<UpdateQuestionCommand>>() { validator };
            _commandBehaviour = new ValidationBehaviour<UpdateQuestionCommand, Gatam.Domain.Question>(validators);
        }

        [TestMethod]
        public async Task Handler_ShouldReturnUpdatedQuestion_WhenValidData()
        {
            //Arrange
            var existingQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = "Wat is je favoriete kleur?", Answers = new List<QuestionAnswer> { new QuestionAnswer { Answer = "Blauw", AnswerValue = "Blue" } } };
            var updatedQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = "Wat is je favoriete kleur?", Answers = new List<QuestionAnswer> { new QuestionAnswer { Answer = "Groen", AnswerValue = "Green" } } };

            var updateCommand = new UpdateQuestionCommand { Id = "123", Question = updatedQuestion };

            _unitOfWork.Setup(uow => uow.QuestionRepository.UpdateQuestionAndAnswers(It.IsAny<Gatam.Domain.Question>())).Returns(Task.CompletedTask);
            _unitOfWork.Setup(uow => uow.Commit()).Returns(Task.CompletedTask);

            //Act
            var result = await _handler.Handle(updateCommand, CancellationToken.None);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Wat is je favoriete kleur?", result.QuestionTitle);
            Assert.AreEqual("Groen",result.Answers.First().Answer); 
        }

        [TestMethod]
        public async Task Handler_ShouldFail_OnEmptyTitle()
        {
            // Arrange
            var invalidQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = "", Answers = new List<QuestionAnswer> { new QuestionAnswer { Answer = "Blauw", AnswerValue = "Blue" } } };
            var updateCommand = new UpdateQuestionCommand { Id = "123", Question = invalidQuestion };

            //Act & Assert
            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(updateCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }
        [TestMethod]
        public async Task Handler_ShouldFail_OnShortTitle()
        {
            // Arrange
            var invalidQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = "Te", Answers = new List<QuestionAnswer> { new QuestionAnswer { Answer = "Blauw", AnswerValue = "Blue" } } };
            var updateCommand = new UpdateQuestionCommand { Id = "123", Question = invalidQuestion };

            //Act & Assert
            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(updateCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }

        [TestMethod]
        public async Task Handler_ShouldFail_OnLongTitle()
        {
            // Arrange
            var invalidQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = new string('a', 513), Answers = new List<QuestionAnswer> { new QuestionAnswer { Answer = "Blauw", AnswerValue = "Blue" } } };
            var updateCommand = new UpdateQuestionCommand { Id = "123", Question = invalidQuestion };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(updateCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }

        [TestMethod]
        public async Task Handler_ShouldFail_OnEmptyAnswers()
        {
            // Arrange
            var invalidQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = "Wat is je favoriete kleur?", Answers = new List<QuestionAnswer>() };
            var updateCommand = new UpdateQuestionCommand { Id = "123", Question = invalidQuestion };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(updateCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }

        [TestMethod]
        public async Task Handler_ShouldFail_OnConflictingModuleId()
        {
            // Arrange
            var invalidQuestion = new Gatam.Domain.Question { Id = "123", QuestionTitle = "Wat is je favoriete kleur?", ApplicationModuleId = "123", Answers = new List<QuestionAnswer>() };
            var updateCommand = new UpdateQuestionCommand { Id = "123", Question = invalidQuestion };

            // Act & Assert
            await Assert.ThrowsExceptionAsync<FailedValidationException>(async () =>
            {
                await _commandBehaviour.Handle(updateCommand, () => Task.FromResult(new Gatam.Domain.Question()), CancellationToken.None);
            });
        }
    }
}
