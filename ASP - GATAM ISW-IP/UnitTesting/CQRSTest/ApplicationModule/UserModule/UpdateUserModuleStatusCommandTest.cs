using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule.UserModule
{
    [TestClass]
    public class UpdateUserModuleStatusCommandTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private UpdateUserModuleStatusHandler _commandHandler;

        [TestInitialize]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _commandHandler = new UpdateUserModuleStatusHandler(_uowMock.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_UpdatesUserModuleState()
        {
            var userModuleId = "module1";
            var newState = UserModuleState.Done;

            var userModule = new Gatam.Domain.UserModule
            {
                Id = userModuleId,
                State = UserModuleState.NotStarted
            };

            _uowMock
                .Setup(repo => repo.UserModuleRepository.FindByIdModuleWithIncludes(userModuleId))
                .ReturnsAsync(userModule);

            var command = new UpdateUserModuleStatusCommand
            {
                UserModuleId = userModuleId,
                State = newState
            };

            var result = await _commandHandler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(newState, result.State);

            _uowMock.Verify(repo => repo.UserModuleRepository.FindByIdModuleWithIncludes(userModuleId), Times.Once);
            _uowMock.Verify(repo => repo.UserModuleRepository.Update(
                It.Is<Gatam.Domain.UserModule>(um => 
                    um.Id == userModuleId && 
                    um.State == newState)), 
                Times.Once);
        }
    }
}
