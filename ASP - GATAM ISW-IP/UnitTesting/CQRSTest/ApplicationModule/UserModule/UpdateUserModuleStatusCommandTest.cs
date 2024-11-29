using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule.UserModule
{
    [TestClass]
    public class UpdateUserModuleStatusCommandTest
    {
        private Mock<IUserModuleRepository> _userModuleRepositoryMock;
        private UpdateUserModuleStatusHandler _commandHandler;

        [TestInitialize]
        public void Setup()
        {
            _userModuleRepositoryMock = new Mock<IUserModuleRepository>();
            _commandHandler = new UpdateUserModuleStatusHandler(_userModuleRepositoryMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_UpdatesUserModuleState()
        {
            var userModuleId = "module1";
            var newState = UserModuleState.InProgress;

            var userModule = new Gatam.Domain.UserModule
            {
                Id = userModuleId,
                State = UserModuleState.NotStarted
            };

            _userModuleRepositoryMock
                .Setup(repo => repo.FindByIdModuleWithIncludes(userModuleId))
                .ReturnsAsync(userModule);

            var command = new UpdateUserModuleStatusCommand
            {
                UserModuleId = userModuleId,
                State = newState
            };

            var result = await _commandHandler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(newState, result.State);

            _userModuleRepositoryMock.Verify(repo => repo.FindByIdModuleWithIncludes(userModuleId), Times.Once);
            _userModuleRepositoryMock.Verify(repo => repo.Update(It.Is<Gatam.Domain.UserModule>(um => um.Id == userModuleId && um.State == newState)), Times.Once);
        }
    }
}
