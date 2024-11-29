using FluentValidation.TestHelper;
using Gatam.Application.CQRS.Questions.Gatam.Application.CQRS.Questions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;

namespace UnitTesting.CQRSTest.Question
{

    [TestClass]
    public class UpdateQuestionPriorityCommandTest
    {
        private readonly UpdateQuestionPriorityCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUserQuestionRepository> _mockUserQuestionRepository;

        public UpdateQuestionPriorityCommandTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserQuestionRepository = new Mock<IUserQuestionRepository>();
            _mockUnitOfWork
                .Setup(uow => uow.UserQuestionRepository)
                .Returns(_mockUserQuestionRepository.Object);

            _validator = new UpdateQuestionPriorityCommandValidator(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public async Task Should_HaveValidationError_When_UserQuestionIdIsEmpty()
        {
            var command = new UpdateQuestionPriorityCommand
            {
                UserQuestionId = string.Empty,
                Priority = QuestionPriority.MEDIUM
            };

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.UserQuestionId)
                .WithErrorMessage("QuestionSetting ID is required");
        }

        [TestMethod]
        public async Task Should_HaveValidationError_When_UserQuestionIdDoesNotExist()
        {
            _mockUserQuestionRepository
                .Setup(repo => repo.GetQuestionSettingById(It.IsAny<string>()))
                .ReturnsAsync((UserQuestion)null);

            var command = new UpdateQuestionPriorityCommand
            {
                UserQuestionId = Guid.NewGuid().ToString(),
                Priority = QuestionPriority.MEDIUM
            };

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.UserQuestionId)
                .WithErrorMessage("Question setting doesn't exist");
        }

        [TestMethod]
        public async Task Should_NotHaveValidationError_When_UserQuestionIdExists()
        {
            _mockUserQuestionRepository
                .Setup(repo => repo.GetQuestionSettingById(It.IsAny<string>()))
                .ReturnsAsync(new UserQuestion());

            var command = new UpdateQuestionPriorityCommand
            {
                UserQuestionId = Guid.NewGuid().ToString(), 
                Priority = QuestionPriority.MEDIUM
            };

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveValidationErrorFor(x => x.UserQuestionId);
        }

        [TestMethod]
        public async Task Should_HaveValidationError_When_PriorityIsInvalid()
        {
            var command = new UpdateQuestionPriorityCommand
            {
                UserQuestionId = Guid.NewGuid().ToString(),
                Priority = (QuestionPriority)999
            };

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Priority)
                .WithErrorMessage("No valid priority given");
        }

        [TestMethod]
        public async Task Should_NotHaveValidationError_When_PriorityIsValid()
        {
            var command = new UpdateQuestionPriorityCommand
            {
                UserQuestionId = Guid.NewGuid().ToString(),
                Priority = QuestionPriority.HIGH 
            };

            var result = await _validator.TestValidateAsync(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Priority);
        }
    }

}
