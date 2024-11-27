using Gatam.Application.CQRS.Module;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.CQRSTest.ApplicationModule
{
    [TestClass]
    public class DeleteModuleCommandTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IUserRepository> _mockUserRepository;
        private DeleteModuleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new DeleteModuleCommandHandler(_mockUnitOfWork.Object, _mockUserRepository.Object);
        }

        [TestMethod]
        public async Task ShouldThrowException_WhenModuleIsInUse()
        {
            // Arrange
            var moduleId = "module123";
            var command = new DeleteModuleCommand { ModuleId = moduleId };

            var userWithModule = new Gatam.Domain.ApplicationUser
            {
                Id = "user1",
                UserModules = new List<Gatam.Domain.UserModule>
            {
                new Gatam.Domain.UserModule { ModuleId = moduleId, State = UserModuleState.InProgress }
            }
            };

            _mockUserRepository
                .Setup(repo => repo.GetUsersByModuleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Gatam.Domain.ApplicationUser> { userWithModule });

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _handler.Handle(command, CancellationToken.None);
            });
        }

       

    }
}
