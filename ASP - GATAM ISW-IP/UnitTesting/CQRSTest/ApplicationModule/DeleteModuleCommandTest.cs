using Gatam.Application.CQRS.Module;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationModule
{
    [TestClass]
    public class DeleteModuleCommandTest
    {
        private Mock<IMediator>? mediator;
        private ModuleController? controller;

        [TestInitialize]
        public void Setup()
        {
            mediator = new Mock<IMediator>();
            controller = new ModuleController(mediator.Object);
        }

        [TestMethod]
        public async Task DeleteModule_DeletesModule()
        {
            // Arrange
            var moduleId = "moduleId123";

            mediator.Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), default)).ReturnsAsync(true);

            // Act
            var result = await controller.DeleteModule(moduleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public async Task DeleteModule_ModuleNotFound_ReturnsOkButIndicatesFailure()
        {
            // Arrange
            var moduleId = "moduleId123";

            // Setup the mock to return false, indicating that the module was not found
            mediator.Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), default)).ReturnsAsync(false);

            // Act
            var result = await controller.DeleteModule(moduleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = (OkObjectResult)result;

            // Check if the response status is 200 OK
            Assert.AreEqual(200, okResult.StatusCode);

            // Check if the response indicates failure (false returned from the handler)
            Assert.IsFalse((bool)okResult.Value);  // Assert that the value returned is false, indicating failure
        }


    }
}

