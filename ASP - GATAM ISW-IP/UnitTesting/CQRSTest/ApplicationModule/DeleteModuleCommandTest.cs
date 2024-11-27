using FluentValidation.TestHelper;
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
        private DeleteModuleCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _validator = new DeleteModuleCommandValidator(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public async Task ShouldFail_WhenModuleIdIsEmpty()
        {
            // Arrange
            var command = new DeleteModuleCommand { ModuleId = "" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ModuleId)
                  .WithErrorMessage("Module ID cannot be empty");
        }

        [TestMethod]
        public async Task ShouldFail_WhenModuleDoesNotExist()
        {
            // Arrange
            var command = new DeleteModuleCommand { ModuleId = "nonexistent" };

            _mockUnitOfWork
                .Setup(u => u.ModuleRepository.FindByIdWithQuestions(It.IsAny<string>()))
                .ReturnsAsync((Gatam.Domain.ApplicationModule)null);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ModuleId)
                  .WithErrorMessage("The module doesnt exist");
        }

        [TestMethod]
        public async Task ShouldFail_WhenModuleIsInUse()
        {
            // Arrange
            var command = new DeleteModuleCommand { ModuleId = "module123" };

            _mockUnitOfWork
                .Setup(u => u.UserRepository.GetUsersByModuleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Gatam.Domain.ApplicationUser>
                {
                    new Gatam.Domain.ApplicationUser
                    {
                        UserModules = new List<Gatam.Domain.UserModule>
                        {
                            new Gatam.Domain.UserModule
                            {
                                ModuleId = "module123",
                                State = UserModuleState.InProgress
                            }
                        }
                    }
                });

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.ModuleId)
                  .WithErrorMessage("De module kan niet worden verwijderd omdat deze in gebruik is door een trajectvolger.");
        }

        [TestMethod]
        public async Task ShouldPass_WhenModuleIdIsValidAndNotInUse()
        {
            // Arrange
            var command = new DeleteModuleCommand { ModuleId = "module123" };

            _mockUnitOfWork
                .Setup(u => u.ModuleRepository.FindByIdWithQuestions(It.IsAny<string>()))
                .ReturnsAsync(new Gatam.Domain.ApplicationModule());

            _mockUnitOfWork
                .Setup(u => u.UserRepository.GetUsersByModuleIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Gatam.Domain.ApplicationUser>());

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

