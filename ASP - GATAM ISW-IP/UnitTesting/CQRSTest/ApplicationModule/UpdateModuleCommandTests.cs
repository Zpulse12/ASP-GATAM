using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
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
    public class UpdateModuleCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private Mock<IUserRepository> _mockUserRepository;
        private UpdateModuleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockUserRepository = new Mock<IUserRepository>();
            _handler = new UpdateModuleCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockUserRepository.Object);
        }

        [TestMethod]
        public async Task ShouldThrowException_WhenModuleIsInUse()
        {
            // Arrange
            var moduleId = "module123";
            var command = new UpdateModuleCommand
            {
                Id = moduleId,
                Module = new ModuleDTO
                {
                    Id = moduleId,
                    Title = "Updated Module",
                    Category = "Updated Category",
                }
            };

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

        [TestMethod]
        public async Task ShouldUpdateModule_WhenValidCommandProvided()
        {
            // Arrange
            var moduleId = "module123";
            var moduleDTO = new ModuleDTO
            {
                Id = moduleId,
                Title = "Updated Module",
                Category = "Updated Category",
            };

            var command = new UpdateModuleCommand
            {
                Id = moduleId,
                Module = moduleDTO
            };

            var applicationModule = new Gatam.Domain.ApplicationModule
            {
                Id = moduleId,
                Title = "Updated Module",
                Category = "Updated Category",
            };

            // Mock voor gebruikers met modules
            var usersWithModule = new List<Gatam.Domain.ApplicationUser>
    {
        new Gatam.Domain.ApplicationUser
        {
            Id = "user1",
            UserModules = new List<Gatam.Domain.UserModule> // Geldige lijst (leeg of gevuld)
            {
                new Gatam.Domain.UserModule
                {
                    ModuleId = moduleId,
                    State = UserModuleState.NotStarted 
                }
            }
        },
        new Gatam.Domain.ApplicationUser
        {
            Id = "user2",
            UserModules = new List<Gatam.Domain.UserModule>()
        }
    };

            // Zorg ervoor dat de mock altijd een lijst retourneert
            _mockUserRepository
                .Setup(repo => repo.GetUsersByModuleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(usersWithModule);

            _mockMapper
                .Setup(mapper => mapper.Map<Gatam.Domain.ApplicationModule>(It.IsAny<ModuleDTO>()))
                .Returns(applicationModule);

            _mockUnitOfWork
                .Setup(uow => uow.ModuleRepository.UpdateModuleWithQuestions(It.IsAny<Gatam.Domain.ApplicationModule>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(uow => uow.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result, "De return waarde van de handler is null.");
            Assert.AreEqual(moduleDTO.Title, result.Title, "De module titel is niet goed geüpdatet.");
            Assert.AreEqual(moduleDTO.Category, result.Category, "De module categorie is niet goed geüpdatet.");

            _mockUnitOfWork.Verify(uow => uow.ModuleRepository.UpdateModuleWithQuestions(It.IsAny<Gatam.Domain.ApplicationModule>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

    }
}
