using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
using FluentValidation;
using Moq;
using Gatam.Application.Behaviours;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class DeleteUserCommandTest
{
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IManagementApi> _managementApi;
    private DeleteUserCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _managementApi = new Mock<IManagementApi>();
        _handler = new DeleteUserCommandHandler(_managementApi.Object, _unitOfWork.Object);
    }

    [TestMethod]
    public async Task DeleteUser_UserExists_ShouldReturnTrue()
    {
        var userId = "testUserId";
        var user = new Gatam.Domain.ApplicationUser { Id = userId };
        
        _unitOfWork.Setup(uow => uow.UserRepository.FindById(userId))
            .ReturnsAsync(user);
        
        _managementApi.Setup(api => api.DeleteUserAsync(userId))
            .ReturnsAsync(true);

        var command = new DeleteUserCommand { UserId = userId };

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result);
        _unitOfWork.Verify(uow => uow.UserRepository.Delete(user), Times.Once);
        _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
    }
}
