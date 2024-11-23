using System.Reflection;
using AutoMapper;
using Gatam.Application.CQRS.Module;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class AssignUserToModuleCommand
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IModuleRepository> _mockModuleRepo;
    private Mock<IMapper> _mockMapper;
    private AssignModulesToUserCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockModuleRepo = new Mock<IModuleRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new AssignModulesToUserCommandHandler(_mockModuleRepo.Object, _mockUnitOfWork.Object, _mockMapper.Object);
    }

    [TestMethod]
    public async Task Handle_ShouldAssignModuleToUser_WhenUserAndModuleExist()
    {
        var user = new Gatam.Domain.ApplicationUser { Id = "user-123", UserModules = new List<UserModule>() };
        var module = new Gatam.Domain.ApplicationModule { Id = "module-456", Questions = new List<Gatam.Domain.Question>() };
        _mockUnitOfWork.Setup(uow => uow.UserRepository.FindById(user.Id)).ReturnsAsync(user);
        _mockModuleRepo.Setup(repo => repo.FindByIdWithQuestions(module.Id)).ReturnsAsync(module);
        await _handler.Handle(new AssignModulesToUserCommand
        {
            FollowerId = user.Id,
            ModuleId = module.Id
        }, CancellationToken.None);
        _mockUnitOfWork.Verify(uow => uow.UserRepository.Update(It.Is<Gatam.Domain.ApplicationUser>(u => 
            u.UserModules.Count == 1)), 
            Times.Once);
        _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
    }

    [TestMethod]
    public async Task Validator_ShouldReturnError_WhenUserDoesNotExist()
    {
        var command = new AssignModulesToUserCommand
        {
            FollowerId = "nonexistent-user",
            ModuleId = "module-123"
        };
        var validator = new AssignModulesToUserCommandValidator(_mockUnitOfWork.Object);

        _mockUnitOfWork.Setup(uow => uow.UserRepository.FindById(It.IsAny<string>()))
            .ReturnsAsync((Gatam.Domain.ApplicationUser)null);

        _mockUnitOfWork.Setup(uow => uow.ModuleRepository.FindById(It.IsAny<string>()))
            .ReturnsAsync((Gatam.Domain.ApplicationModule)null);

        var validationResult = await validator.ValidateAsync(command);
        Assert.IsFalse(validationResult.IsValid);
        Assert.IsTrue(validationResult.Errors.Exists(e => e.ErrorMessage == "The user does not exist"));
    }
}

