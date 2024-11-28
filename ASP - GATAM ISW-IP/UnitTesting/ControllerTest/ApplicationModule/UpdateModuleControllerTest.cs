using Gatam.Application.CQRS.DTOS.ModulesDTO;
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
    public class UpdateModuleControllerTest
    {
        private Mock<IMediator>? _mediator;
        private ModuleController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();
            _controller = new ModuleController(_mediator.Object);
        }

        [TestMethod]
        public async Task UpdateModule_ShouldReturnOk_WhenValidCommandProvided()
        {
            // Arrange
            var moduleId = "module123";
            var moduleDTO = new ModuleDTO
            {
                Id = moduleId,
                Title = "Updated Module",
                Category = "Updated Category",
            };

            var returnedModuleDTO = new ModuleDTO  
            {
                Id = moduleId,
                Title = "Updated Module",
                Category = "Updated Category",
            };

            // Mock de mediator call
            _mediator.Setup(m => m.Send(It.IsAny<UpdateModuleCommand>(), default))
                     .ReturnsAsync(returnedModuleDTO);  

            // Act
            var result = await _controller.UpdateModule(moduleId, moduleDTO);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "De return waarde is geen OkResult.");
            Assert.AreEqual(200, okResult.StatusCode, "De statuscode is niet OK.");
            Assert.AreEqual(returnedModuleDTO, okResult.Value, "De module is niet goed geüpdatet.");
        }

    }
}
