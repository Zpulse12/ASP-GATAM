using Gatam.Application.CQRS.Module;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.ApplicationModule
{
    [TestClass]
    public class GetAllModulesQueryTest
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
        public async Task GetAllModules_ReturnsOkResult_WithListOfModules()
        {
            // Arrange
            var expectedModules = new List<Gatam.Application.CQRS.DTOS.ModulesDTO.ModuleDTO>
            {
                new Gatam.Application.CQRS.DTOS.ModulesDTO.ModuleDTO { Title = "Module1", Category = "Category1" },
                new Gatam.Application.CQRS.DTOS.ModulesDTO.ModuleDTO { Title = "Module2", Category = "Category2" }
            };

            _mediatorMock
        .Setup(m => m.Send(It.IsAny<GetAllModulesQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedModules);

            // Act
            var result = await _moduleController.GetAllModules();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Expected OkObjectResult but got null.");
            Assert.AreEqual(200, okResult.StatusCode, "Expected status code 200.");

            var modules = okResult.Value as List<Gatam.Application.CQRS.DTOS.ModulesDTO.ModuleDTO>;
            Assert.IsNotNull(modules, "Expected a list of modules in the response.");
            Assert.AreEqual(expectedModules.Count, modules.Count, "Module count does not match.");
        }

    }
}
