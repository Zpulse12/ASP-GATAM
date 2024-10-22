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
        private Mock<IValidator<DeactivateUserCommand>>? _validatorMock;


        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>();
            _unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(_userRepositoryMock.Object);
            _mapperMock = new Mock<IMapper>();
            _validatorMock = new Mock<IValidator<DeactivateUserCommand>>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeactivateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
           _handler= new DeactivateUserCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object,_validatorMock.Object);
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
        public async Task Handle_Should_Throw_ValidationException_When_Validation_Fails()
        {
            var command = new DeactivateUserCommand { _userId = "123", IsActive = false };
            var validationFailure = new ValidationFailure("_userId", "Invalid user ID");
            var validationResult = new ValidationResult(new List<ValidationFailure> { validationFailure });
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeactivateUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);
            await Assert.ThrowsExceptionAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));
        }
       

    }

}
