using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Application.Interfaces;
using Moq;
using FluentValidation.TestHelper;
using Gatam.Application.CQRS;
using AutoMapper;

namespace UnitTesting.CQRSTest.ApplicationUser
{
   

    [TestClass]
    public class UnassignUserCommandTest
    {
        private Mock<IUnitOfWork> _uowMock;
        private Mock<IMapper> _mapperMock;
        private UnassignUserCommandHandler _handler;


        [TestInitialize]
        public void Setup()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UnassignUserCommandHandler(_uowMock.Object, _mapperMock.Object);
        }

        [TestMethod]
        public void Validate_VolgerIdIsEmpty_ReturnsValidationError()
        {
            var validator = new UnassignUserCommandValidator();
            var command = new UnassignUserCommand { FollowerId = "" };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.FollowerId)
                  .WithErrorMessage("VolgerId mag niet leeg zijn");
        }


    }

}
