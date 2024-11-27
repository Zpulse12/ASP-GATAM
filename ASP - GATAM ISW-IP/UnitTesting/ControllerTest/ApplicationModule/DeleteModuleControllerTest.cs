using Gatam.Application.CQRS.Module;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.ApplicationModule
{
    [TestClass]
    public class DeleteModuleControllerTest
    {
        private Mock<IMediator> _mockMediator;
        private ModuleController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new ModuleController(_mockMediator.Object); // Replace YourController with your actual controller name
        }

        [TestMethod]
        public async Task ShouldReturnOk_WhenModuleIsDeletedSuccessfully()
        {
            // Arrange
            var moduleId = "module123";
            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); // Simulate successful module deletion

            // Act
            var result = await _controller.DeleteModule(moduleId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); // Check for HTTP 200 status code
            Assert.AreEqual(true, okResult.Value); // Check the response value
        }

        [TestMethod]
        public async Task ShouldReturnNotFound_WhenModuleDoesNotExist()
        {
            // Arrange
            var moduleId = "module123";
            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); // Simulate module not found

            // Act
            var result = await _controller.DeleteModule(moduleId);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode); // Check for HTTP 404 status code
            Assert.AreEqual("Module doesn't exist", notFoundResult.Value); // Check the response message
        }
    }
}
