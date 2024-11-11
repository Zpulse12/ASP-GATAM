using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser
{
    [TestClass]
    public class CreateUserCommandTest
    {
        private Mock<IUnitOfWork>? _unitOfWorkMock;
        private Mock<IUserRepository>? _userRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(_userRepositoryMock.Object);
        }
    }
}
