using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule.UserModule
{
    [TestClass]
    public class SubmitUserAnswersCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IGenericRepository<UserAnswer>> _userAnswerRepositoryMock;
        private SubmitUserAnswersCommandHandler _commandHandler;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userAnswerRepositoryMock = new Mock<IGenericRepository<UserAnswer>>();

            _unitOfWorkMock.SetupGet(uow => uow.UserAnwserRepository).Returns(_userAnswerRepositoryMock.Object);

            _commandHandler = new SubmitUserAnswersCommandHandler(_unitOfWorkMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_UpdatesAndAddsAnswers()
        {
            // Arrange
            var userModuleId = "module1";
            var existingAnswer = new UserAnswer { Id = "answer1", GivenAnswer = "Old Answer" };
            var newAnswerToUpdate = new UserAnswer { Id = "answer1", GivenAnswer = "Updated Answer" };
            var newAnswerToAdd = new UserAnswer { Id = "answer2", GivenAnswer = "New Answer" };

            var userModule = new Gatam.Domain.UserModule
            {
                Id = userModuleId,
                UserGivenAnswers = new List<UserAnswer> { existingAnswer }
            };

            _unitOfWorkMock
                .Setup(repo => repo.UserModuleRepository.FindById(userModuleId))
                .ReturnsAsync(userModule);

            _userAnswerRepositoryMock
                .Setup(repo => repo.FindById(existingAnswer.Id))
                .ReturnsAsync(existingAnswer);

            var command = new SubmitUserAnswersCommand
            {
                UserModuleId = userModuleId,
                UserAnwsers = new List<UserAnswer> { newAnswerToUpdate, newAnswerToAdd }
            };

            // Act
            var result = await _commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(a => a.Id == existingAnswer.Id && a.GivenAnswer == "Updated Answer"));
            Assert.IsTrue(result.Any(a => a.Id == newAnswerToAdd.Id && a.GivenAnswer == "New Answer"));

            _userAnswerRepositoryMock.Verify(repo => repo.Update(It.Is<UserAnswer>(a => a.Id == existingAnswer.Id && a.GivenAnswer == "Updated Answer")), Times.Once);
            _userAnswerRepositoryMock.Verify(repo => repo.Create(It.Is<UserAnswer>(a => a.Id == newAnswerToAdd.Id && a.GivenAnswer == "New Answer")), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
        }

        [TestMethod]
        public async Task Handle_NoAnswersToAddOrUpdate_CommitsSuccessfully()
        {
            // Arrange
            var userModuleId = "module1";
            var existingAnswer = new UserAnswer { Id = "answer1", GivenAnswer = "Same Answer" };

            var userModule = new Gatam.Domain.UserModule
            {
                Id = userModuleId,
                UserGivenAnswers = new List<UserAnswer> { existingAnswer }
            };

            _unitOfWorkMock
                .Setup(repo => repo.UserModuleRepository.FindById(userModuleId))
                .ReturnsAsync(userModule);

            _userAnswerRepositoryMock
                .Setup(repo => repo.FindById(existingAnswer.Id))
                .ReturnsAsync(existingAnswer);

            var command = new SubmitUserAnswersCommand
            {
                UserModuleId = userModuleId,
                UserAnwsers = new List<UserAnswer> { existingAnswer }
            };

            // Act
            var result = await _commandHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Any(a => a.Id == existingAnswer.Id && a.GivenAnswer == "Same Answer"));

            _userAnswerRepositoryMock.Verify(repo => repo.Update(It.IsAny<UserAnswer>()), Times.Never);
            _userAnswerRepositoryMock.Verify(repo => repo.Create(It.IsAny<UserAnswer>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
        }
    }
}
