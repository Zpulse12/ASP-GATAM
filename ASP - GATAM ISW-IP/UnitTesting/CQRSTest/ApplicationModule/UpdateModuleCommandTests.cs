﻿using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Module;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule
{
    [TestClass]
    public class UpdateModuleCommandTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private UpdateModuleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _handler = new UpdateModuleCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object);
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
                Category = "Updated Category"
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
                Category = "Updated Category"
            };

            _mockUnitOfWork
                .Setup(uow => uow.ModuleRepository.FindByProperty("Title", moduleDTO.Title))
                .ReturnsAsync((Gatam.Domain.ApplicationModule)null); // Geen duplicaten

            _mockMapper
                .Setup(mapper => mapper.Map<Gatam.Domain.ApplicationModule>(moduleDTO))
                .Returns(applicationModule);

          
            _mockUnitOfWork.Setup(uow => uow.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result, "De return waarde van de handler is null.");
            Assert.AreEqual(moduleDTO.Title, result.Title, "De module titel is niet goed geüpdatet.");
            Assert.AreEqual(moduleDTO.Category, result.Category, "De module categorie is niet goed geüpdatet.");

            _mockUnitOfWork.Verify(uow => uow.ModuleRepository.Update(applicationModule), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }
    }
}
