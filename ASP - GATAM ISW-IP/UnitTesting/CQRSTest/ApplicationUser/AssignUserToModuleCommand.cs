// using Gatam.Application.CQRS.Module;
// using Gatam.Application.Interfaces;
// using Gatam.Domain;
// using Moq;
//
// namespace UnitTesting.CQRSTest.ApplicationUser;
// [TestClass]
// public class AssignUserToModuleCommand
// {
//  private Mock<IUnitOfWork> _mockUnitOfWork;
//         private AssignModulesToUserCommandHandler _handler;
//
//         [TestInitialize]
//         public void Setup()
//         {
//             _mockUnitOfWork = new Mock<IUnitOfWork>();
//             _handler = new AssignModulesToUserCommandHandler(_mockUnitOfWork.Object);
//         }
//
//         [TestMethod]
//         public async Task Handle_ShouldAssignModuleToUser_WhenUserAndModuleExist()
//         {
//             var userId = "user-123";
//             var moduleId = "module-456";
//
//             var user = new Gatam.Domain.ApplicationUser()
//             {
//                 Id = userId,
//                 UserModules = new List<UserModule>()
//             };
//
//             var module = new Gatam.Domain.ApplicationModule
//             {
//                 Id = moduleId
//             };
//
//             _mockUnitOfWork.Setup(uow => uow.UserRepository.FindById(userId))
//                 .ReturnsAsync(user);
//
//             _mockUnitOfWork.Setup(uow => uow.ModuleRepository.FindById(moduleId))
//                 .ReturnsAsync(module);
//
//             var result = await _handler.Handle(new AssignModulesToUserCommand
//             {
//                 VolgerId = userId,
//                 ModuleId = moduleId
//             }, CancellationToken.None);
//
//             Assert.IsTrue(result);
//             _mockUnitOfWork.Verify(uow => uow.UserRepository.Update(It.Is<Gatam.Domain.ApplicationUser>(u => u.UserModules.Count == 1)), Times.Once);
//             _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once);
//         }
//         
//         [TestMethod]
//         public async Task Validator_ShouldReturnError_WhenUserDoesNotExist()
//         {
//             var command = new AssignModulesToUserCommand
//             {
//                 VolgerId = "nonexistent-user",
//                 ModuleId = "module-123"
//             };
//
//             var validator = new AssignModulesToUserCommandValidator(_mockUnitOfWork.Object);
//
//             _mockUnitOfWork.Setup(uow => uow.UserRepository.FindById(It.IsAny<string>()))
//                 .ReturnsAsync((Gatam.Domain.ApplicationUser)null);
//
//             _mockUnitOfWork.Setup(uow => uow.ModuleRepository.FindById(It.IsAny<string>()))
//                 .ReturnsAsync((Gatam.Domain.ApplicationModule)null);
//             var validationResult = await validator.ValidateAsync(command);
//             Assert.IsFalse(validationResult.IsValid);
//             Assert.IsTrue(validationResult.Errors.Exists(e => e.ErrorMessage == "The user does not exist"));
//         } 
// }
//
