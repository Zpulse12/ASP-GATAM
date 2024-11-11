using Gatam.Application.CQRS.Module;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationModule
{
    [TestClass]
    public class CreateModuleControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private ModuleController _moduleController;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _moduleController = new ModuleController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task CreateModule_ReturnsCreatedResult_WithModuleData()
        {
            var newModule = new Gatam.Domain.ApplicationModule { Title = "New Module", Category = "Test Category" };
            var command = new CreateModuleCommand { _module = newModule };
            _mediatorMock
                .Setup(m => m.Send(It.Is<CreateModuleCommand>(c => c._module == newModule), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newModule);

            var result = await _moduleController.CreateModule(newModule);

            var createdResult = result as CreatedResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(201, createdResult.StatusCode);
            Assert.AreEqual(newModule, createdResult.Value);
        }
    }
}
