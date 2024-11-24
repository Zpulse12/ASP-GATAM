using Gatam.Application.CQRS;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Application.CQRS.User.Roles;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class DeleteUserRolesCommandTest
    {
        private Mock<IManagementApi> _managementApiMock;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IUserRepository> _userRepositoryMock;
        private DeleteUserRolesCommandValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _managementApiMock = new Mock<IManagementApi>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWork.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
            _validator = new DeleteUserRolesCommandValidator(_managementApiMock.Object, _unitOfWork.Object);
        }

        [TestMethod]
        public async Task Should_Fail_When_UserId_Is_Empty()
        {
            const string userId = "";
            _userRepositoryMock.Setup(repo => repo.FindById(userId)).ReturnsAsync((Gatam.Domain.ApplicationUser)null);
            _managementApiMock.Setup(api => api.GetUserByIdAsync(userId)).ReturnsAsync((UserDTO)null);

            var command = new DeleteUserRolesCommand
            {
                UserId = userId,
                Roles = new RolesDTO { Roles = new List<string> { "Admin" } }
            };
            var result = await _validator.ValidateAsync(command);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.PropertyName == "UserId" && e.ErrorMessage == "User ID cannot be empty"));
        }

        [TestMethod]
        public async Task Should_Fail_When_Roles_Are_Empty()
        {
            const string userId = "123";
            _userRepositoryMock.Setup(repo => repo.FindById(userId)).ReturnsAsync(new Gatam.Domain.ApplicationUser());
            _managementApiMock.Setup(api => api.GetUserByIdAsync(userId)).ReturnsAsync(new UserDTO());

            var command = new DeleteUserRolesCommand
            {
                UserId = userId,
                Roles = new RolesDTO { Roles = new List<string>() }
            };
            var result = await _validator.ValidateAsync(command);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any());
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

