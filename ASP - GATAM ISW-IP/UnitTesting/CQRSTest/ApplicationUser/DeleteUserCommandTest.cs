using FluentValidation;
using FluentValidation.Results;
using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class DeleteUserCommandTest
{
    private Mock<IUnitOfWork>? _unitOfWorkMock;
    private Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>? _userRepositoryMock;
    private DeleteUserCommandHandler? _handler;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IGenericRepository<Gatam.Domain.ApplicationUser>>();
        _unitOfWorkMock.Setup(mock => mock.UserRepository).Returns(_userRepositoryMock.Object);
        _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);

    }
    [TestMethod]
    public async Task DeleteUser_ValidUserIdButUserNotFound_ShouldReturnFalse()
    {
        const string userId = "oopsdoopsie";
        var command = new DeleteUserCommand { UserId = userId };
        _userRepositoryMock
            .Setup(repo => repo.FindById(userId))
            .ReturnsAsync((Gatam.Domain.ApplicationUser)null); 
        var result = await _handler!.Handle(command, CancellationToken.None);
        Assert.IsFalse(result);
        _userRepositoryMock.Verify(repo => repo.FindById(userId), Times.Once);
    }
    [TestMethod]
    public async Task DeleteUser_UserExists_ShouldReturnTrue()
    {
        const string userId = "user123";
        var user = new Gatam.Domain.ApplicationUser { Id = userId };
        _userRepositoryMock
            .Setup(repo => repo.FindById(userId))
            .ReturnsAsync(user);
        _userRepositoryMock
            .Setup(repo => repo.Delete(user))
            .Returns(Task.CompletedTask);
        var command = new DeleteUserCommand { UserId = userId };
        var result = await _handler!.Handle(command, CancellationToken.None);
        Assert.IsTrue(result);
        _userRepositoryMock.Verify(repo => repo.FindById(userId), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Delete(user), Times.Once);
    }
}