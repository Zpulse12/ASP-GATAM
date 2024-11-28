using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Domain;
using Gatam.Application.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation.TestHelper;
using Gatam.Application.CQRS;

namespace UnitTesting.CQRSTest.ApplicationUser
{
   

    [TestClass]
    public class UnassignUserCommandTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private UnassignUserCommandHandler _handler;


        [TestInitialize]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();

            _handler = new UnassignUserCommandHandler(_uowMock.Object);
        }
        [TestMethod]
        public void Validate_UserIsNull_ReturnsValidationError()
        {
            var validator = new UnassignUserCommandValidator(); // Create a new instance of the validator
            var command = new UnassignUserCommand { VolgerId = "validVolgerId", User = null };

            // Act
            var result = validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(c => c.User)
                  .WithErrorMessage("Gebruiker mag niet null zijn");
        }

        [TestMethod]
        public void Validate_VolgerIdIsEmpty_ReturnsValidationError()
        {
            var validator = new UnassignUserCommandValidator();
            var command = new UnassignUserCommand { VolgerId = "", User = new UserDTO() };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.VolgerId)
                  .WithErrorMessage("VolgerId mag niet leeg zijn");
        }


    }

}
