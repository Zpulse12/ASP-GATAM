using FluentValidation;
using Gatam.Application.CQRS.Module;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTesting.CQRSTest.ApplicationModule
{
    [TestClass]
    public class CreateModuleCommandTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private CreateModuleCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateModuleCommandHandler(_unitOfWorkMock.Object);
        }

        [TestMethod]
        public async Task Handle_ShouldCreateModule_WhenCommandIsValid()
        {
            var module = new Gatam.Domain.ApplicationModule { Title = "Test Module", Category = "Test Category" };
            var command = new CreateModuleCommand { _module = module };

            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.Create(module)).ReturnsAsync(module);
            _unitOfWorkMock.Setup(uow => uow.commit()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(uow => uow.ModuleRepository.Create(module), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.commit(), Times.Once);
            Assert.AreEqual(module, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task Handle_ShouldThrowValidationException_WhenModuleIsInvalid()
        {
            var module = new Gatam.Domain.ApplicationModule { Title = "", Category = "Test Category" }; // Ongeldige data
            var command = new CreateModuleCommand { _module = module };

            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.Create(It.IsAny<Gatam.Domain.ApplicationModule>()))
                .ThrowsAsync(new ValidationException("Module validation failed"));

            await _handler.Handle(command, CancellationToken.None);
        }

        [TestMethod]
        public async Task Handle_ShouldReturnModule_WhenCommitIsSuccessful()
        {
            var module = new Gatam.Domain.ApplicationModule { Title = "Test Module", Category = "Test Category" };
            var command = new CreateModuleCommand { _module = module };

            _unitOfWorkMock.Setup(uow => uow.ModuleRepository.Create(module)).ReturnsAsync(module);
            _unitOfWorkMock.Setup(uow => uow.commit()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(module.Title, result.Title);
            Assert.AreEqual(module.Category, result.Category);
        }
    }
}
