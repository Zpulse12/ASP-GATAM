using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;


namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class DeactivateUserCommandTest
    {
        private Mock<IUnitOfWork>? _unitOfWorkMock;
        private Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>? _userRepositoryMock;
        private DeactivateUserCommandHandler? _handler;
        private Mock<IMapper>? _mapperMock;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>();
            _unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(_userRepositoryMock.Object);
            _mapperMock = new Mock<IMapper>();
           _handler= new DeactivateUserCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public async Task DeactivateUserCommand_ShouldDeactivateUser_WhenUserExists()
        {
            var _userId = "123";
            var user = new Gatam.Domain.ApplicationUser { Id = _userId, IsActive=true };
            var command = new DeactivateUserCommand { _userId = _userId, IsActive=false };

            _userRepositoryMock.Setup(repo => repo.FindById(_userId)).ReturnsAsync(user); 

            var result = await _handler!.Handle(command, CancellationToken.None);

       
            _userRepositoryMock.Verify(repo => repo.Update(It.Is<Gatam.Domain.ApplicationUser>(u =>u.Id == _userId && u.IsActive == false)), Times.Once);

           
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public async Task UserIdIsNull_ShouldHaveValidationError()
        {
            var command = new DeactivateUserCommand { _userId = null, IsActive = false };
            var validator = new DeactivateUserValidation(_unitOfWorkMock.Object);

            var result = await validator.TestValidateAsync(command);
            result.ShouldHaveValidationErrorFor(c => c._userId)
                .WithErrorCode("NotEmptyValidator");
        }
       

    }

}
