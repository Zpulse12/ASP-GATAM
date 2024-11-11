﻿using Gatam.Application.CQRS.User;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.ApplicationUser
{
    [TestClass]
    public class AssignUsersToBegeleiderControllerTest
    {
        private Mock<IMediator> _mediatorMock;
        private UserController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        [TestMethod]
        public async Task AssignUsersToBegeleider_UserExists_ReturnsOkResult()
        {
            var user = new Gatam.Domain.ApplicationUser { Id = "volgerId" };
            var begeleiderId = "begeleiderId";

            _mediatorMock.Setup(m => m.Send(It.IsAny<FindUserByIdQuery>(), default))
                         .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId" });  

            _mediatorMock.Setup(m => m.Send(It.IsAny<Gatam.Application.CQRS.User.BegeleiderAssignment.AssignUserToBegeleiderCommand>(), default))
                         .ReturnsAsync(new Gatam.Domain.ApplicationUser { Id = "volgerId", BegeleiderId = begeleiderId }); 

            var result = await _controller.AssignUsersToBegeleider(user, begeleiderId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);  
        }

        [TestMethod]
        public async Task AssignUsersToBegeleider_UserNotFound_ReturnsNotFoundResult()
        {
            var user = new Gatam.Domain.ApplicationUser { Id = "volgerId" };
            var begeleiderId = "begeleiderId";

            _mediatorMock.Setup(m => m.Send(It.IsAny<FindUserByIdQuery>(), default))
                         .ReturnsAsync((Gatam.Domain.ApplicationUser)null);

            var result = await _controller.AssignUsersToBegeleider(user, begeleiderId);

            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);  
        }
    }
}
