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
            _controller = new ModuleController(_mockMediator.Object); 
        }

        [TestMethod]
        public async Task ShouldReturnOk_WhenModuleIsDeletedSuccessfully()
        {
            // Arrange
            var moduleId = "module123";
            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true); 

            // Act
            var result = await _controller.DeleteModule(moduleId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); 
            Assert.AreEqual(true, okResult.Value);
        }

        [TestMethod]
        public async Task ShouldReturnOk_WhenModuleDoesNotExist()
        {
            // Arrange
            var moduleId = "module123";
            _mockMediator
                .Setup(m => m.Send(It.IsAny<DeleteModuleCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false); 

            // Act
            var result = await _controller.DeleteModule(moduleId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult); 
            Assert.AreEqual(200, okResult.StatusCode); 
            Assert.AreEqual(false, okResult.Value); 
        }
    }
}
