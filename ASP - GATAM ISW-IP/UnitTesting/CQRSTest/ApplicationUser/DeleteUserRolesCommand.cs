using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Application.CQRS.User.Roles;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class DeleteUserRolesCommandTest
    {
        private Mock<IManagementApi> _managementApi;
        private Mock<IUnitOfWork> _unitOfWork;
        private DeleteUserRolesCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _managementApi = new Mock<IManagementApi>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _validator = new DeleteUserRolesCommandValidator(_managementApi.Object, _unitOfWork.Object);
        }

        [TestMethod]
        public async Task Should_Fail_When_UserId_Is_Empty()
        {
            var command = new DeleteUserRolesCommand
            {
                UserId = string.Empty,
                Roles = new RolesDTO() { Roles = new List<string> { "Admin" } }
            };
            var result = await _validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "UserId" && e.ErrorMessage == "User ID cannot be empty"));
        }

        [TestMethod]
        public async Task Should_Fail_When_Roles_Are_Empty()
        {
            var command = new DeleteUserRolesCommand
            {
                UserId = "123",
                Roles = new RolesDTO() { Roles = new List<string>() }
            };
            var result = await _validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "Roles" && e.ErrorMessage == "Roles cannot be empty"));
        }

        [TestMethod]
        public async Task Should_Fail_When_User_Does_Not_Exist_In_UnitOfWork()
        {
            _unitOfWork.Setup(uow => uow.UserRepository.FindById(It.IsAny<string>())).ReturnsAsync((Gatam.Domain.ApplicationUser)null);

            var command = new DeleteUserRolesCommand
            {
                UserId = "123",
                Roles = new RolesDTO() { Roles = new List<string> { "Admin" } }
            };
            var result = await _validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "UserId" && e.ErrorMessage == "The user doesnt exist"));
        }
    }
}

