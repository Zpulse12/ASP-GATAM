using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            var expectedModules = new List<Gatam.Domain.ApplicationModule>
            {
                new Gatam.Domain.ApplicationModule { Title = "Module1", Category = "Category1" },
                new Gatam.Domain.ApplicationModule { Title = "Module2", Category = "Category2" }
            };
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<Gatam.Application.CQRS.Module.GetAllModulesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedModules);

            var result = await _moduleController.GetAllModules();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            var modules = okResult.Value as List<Gatam.Domain.ApplicationModule>;
            Assert.IsNotNull(modules);
            Assert.AreEqual(expectedModules.Count, modules.Count);
        }

    }
}
