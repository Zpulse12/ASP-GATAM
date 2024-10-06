using FluentValidation.TestHelper;
using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ApplicationUser
{
    [TestClass]
    public class CreateUserCommandTest
    {
        private Mock<IUnitOfWork>? _unitOfWorkMock;
        private Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>? _userRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>();
            _unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(_userRepositoryMock.Object);
        }
        [TestMethod]
        public async Task EmailIsNull()
        {
            Gatam.Domain.ApplicationUser user = new Gatam.Domain.ApplicationUser() { Email = null };
            Gatam.Application.CQRS.CreateUserCommand unit = new Gatam.Application.CQRS.CreateUserCommand() { _user = user };
            var result = await new Gatam.Application.CQRS.CreateUserCommandValidator(_unitOfWorkMock.Object).TestValidateAsync(user);
            result.ShouldHaveValidationErrorFor(validationObject => validationObject.Email)
                .WithErrorCode("NotNullValidator");
        }
        [TestMethod]
        public async Task EmailIsEmpty()
        {
            Gatam.Domain.ApplicationUser user = new Gatam.Domain.ApplicationUser() { Email = "" };
            Gatam.Application.CQRS.CreateUserCommand unit = new Gatam.Application.CQRS.CreateUserCommand() { _user = user };
            var result = await new Gatam.Application.CQRS.CreateUserCommandValidator(_unitOfWorkMock.Object).TestValidateAsync(user);
            result.ShouldHaveValidationErrorFor(validationObject => validationObject.Email)
                .WithErrorCode("NotEmptyValidator");
        }
    }
}
