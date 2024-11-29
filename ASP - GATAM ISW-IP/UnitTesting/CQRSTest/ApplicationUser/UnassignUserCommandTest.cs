using Gatam.Application.CQRS.User.BegeleiderAssignment;
using Gatam.Application.Interfaces;
using Moq;
using AutoMapper;
using FluentValidation.TestHelper;

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
        public async Task Handle_UserExists_UnassignsUserSuccessfully()
        {
            // Arrange
            var volgerId = "volgerId";
            var user = new Gatam.Domain.ApplicationUser
            {
                Id = volgerId,
                BegeleiderId = "begeleiderId"
            };

            _uowMock.Setup(u => u.UserRepository.FindById(volgerId)).ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(new UnassignUserCommand { VolgerId = volgerId, User = user }, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(volgerId, result.Id);
            Assert.IsNull(result.BegeleiderId);

            _uowMock.Verify(u => u.UserRepository.Update(It.Is<Gatam.Domain.ApplicationUser>(x => x.BegeleiderId == null)), Times.Once);
            _uowMock.Verify(u => u.Commit(), Times.Once);
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
            var command = new UnassignUserCommand { VolgerId = "", User = new Gatam.Domain.ApplicationUser() };
            var result = validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.VolgerId)
                  .WithErrorMessage("VolgerId mag niet leeg zijn");
        }


    }

}
