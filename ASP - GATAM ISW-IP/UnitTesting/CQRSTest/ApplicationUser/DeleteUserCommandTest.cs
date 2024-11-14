
using FluentValidation;
using FluentValidation.Results;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class DeleteUserCommandTest
{
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IGenericRepository<Gatam.Domain.ApplicationUser>> _userRepositoryMock;
    private Mock<IManagementApi> _managementApiMock;
    private DeleteUserCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _managementApiMock = new Mock<IManagementApi>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>();

        _unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(_userRepositoryMock.Object);
        _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object, _managementApiMock.Object);
    }

    [TestMethod]
    public async Task DeleteUser_UserNotFound_ShouldReturnFalse()
    {
        const string userId = "nonexistentUser";
        _userRepositoryMock.Setup(repo => repo.FindById(userId)).ReturnsAsync((Gatam.Domain.ApplicationUser)null);
        _managementApiMock.Setup(api => api.GetUserByIdAsync(userId)).ReturnsAsync((UserDTO)null);

        var command = new DeleteUserCommand { UserId = userId };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result);
        _userRepositoryMock.Verify(repo => repo.FindById(userId), Times.Once);
    }

    [TestMethod]
    public async Task DeleteUser_UserExists_ShouldReturnTrue()
    {
        const string userId = "user123";
        var user = new Gatam.Domain.ApplicationUser { Id = userId };
        var userDto = new UserDTO
        {
            Id = userId,
            Name = null,
            Surname= null,
            PhoneNumber= null,
            Email = null,
            RolesIds = null
        };

        _userRepositoryMock.Setup(repo => repo.FindById(userId)).ReturnsAsync(user);
        _managementApiMock.Setup(api => api.GetUserByIdAsync(userId)).ReturnsAsync(userDto);
        _managementApiMock.Setup(api => api.DeleteUserAsync(userId)).ReturnsAsync(true);

        var command = new DeleteUserCommand { UserId = userId };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result);
        _userRepositoryMock.Verify(repo => repo.FindById(userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(user), Times.Once);
    }

    [TestMethod]
    public async Task DeleteUser_Auth0DeleteFails_ShouldReturnFalse()
    {
        const string userId = "user123";
        var user = new Gatam.Domain.ApplicationUser { Id = userId };
        var userDto = new UserDTO
        {
            Id = userId,
            Name = null,
            Surname=null,
            PhoneNumber= null,
            Email = null,
            RolesIds = null
        };

        _userRepositoryMock.Setup(repo => repo.FindById(userId)).ReturnsAsync(user);
        _managementApiMock.Setup(api => api.GetUserByIdAsync(userId)).ReturnsAsync(userDto);
        _managementApiMock.Setup(api => api.DeleteUserAsync(userId)).ReturnsAsync(false);

        var command = new DeleteUserCommand { UserId = userId };
        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsFalse(result);
        _userRepositoryMock.Verify(repo => repo.FindById(userId), Times.Once);
    }
}
