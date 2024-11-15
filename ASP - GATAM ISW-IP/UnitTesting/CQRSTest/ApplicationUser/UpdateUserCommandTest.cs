//using Gatam.Application.CQRS;
//using Gatam.Application.CQRS.User;
//using Gatam.Application.Extensions;
//using Gatam.WebAPI.Controllers;
//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Moq;

//namespace UnitTesting.ControllerTest.ApplicationUser;
//[TestClass]
//public class UpdateUserControllerTest
//{
//    private Mock<IMediator>? mediator;
//    private UserController? controller;

//    [TestInitialize]
//    public void Setup()
//    {
//        mediator = new Mock<IMediator>();
//        controller = new UserController(mediator.Object);
//    }

//    [TestMethod]
//    public async Task ShouldUpdateNickname_WhenNicknameChanges()
//    {
//        // Arrange
//        var userId = "12345";
//        var originalUser = new Gatam.Domain.ApplicationUser
//        {
//            Id = userId,
//            Name = "OldNickname", // Oud Nickname
//            Email = "user@example.com",
//            IsActive = true
//        };

//        var updatedUserDto = new UserDTO
//        {
//            Id = userId,
//            Name = "NewNickname",  // Nieuwe Nickname
//            Surname ="NewSurname",
//            Email = originalUser.Email,
//            IsActive = true,
//            RolesIds = new List<string> { RoleMapper.Roles["BEHEERDER"] }
//        };

//        //  ophalen van de originele gebruiker
//        _mockUnitOfWork.Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
//                       .ReturnsAsync(originalUser);

//        // Mock de Auth0 API interactie om de nickname te updaten
//        _mockAuth0Repository.Setup(x => x.UpdateUserNicknameAsync(It.Is<UserDTO>(u => u.Name == "NewNickname")))
//                            .ReturnsAsync(updatedUserDto);  // Retourneer de bijgewerkte UserDTO

//        // Mock de return van de UpdateUserCommandHandler
//        _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<UserDTO>())).Returns(updatedUserDto);

//        // Act
//        var command = new UpdateUserCommand
//        {
//            Id = userId,
//            User = updatedUserDto
//        };

//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        Assert.IsNotNull(result, "De return waarde van de handler is null.");
//        Assert.AreEqual("NewNickname", result.Name, "De nickname is niet goed geüpdatet.");
//        Assert.AreEqual(originalUser.Email, result.Email, "De email zou niet veranderd moeten zijn.");
//    }


//    [TestMethod]
//    public async Task ShouldUpdateEmail_WhenEmailChanges()
//    {
//        // Arrange
//        var userId = "12345";
//        var originalUser = new Gatam.Domain.ApplicationUser
//        {
//            Id = userId,
//            Name = "OldNickname",
//            Email = "user@example.com", // Oude Email
//            IsActive = true
//        };

//        var updatedUserDto = new UserDTO
//        {
//            Id = userId,
//            Name = originalUser.Name,
//            Surname = originalUser.Email,
//            Email = "newemail@example.com",  // Nieuwe Email
//            IsActive = true,
//            RolesIds = new List<string> { RoleMapper.Roles["BEHEERDER"] },
//        };

//        //ophalen van de originele gebruiker
//        _mockUnitOfWork.Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
//                       .ReturnsAsync(originalUser);

//        // Mock de interactie met de Auth0 API om de email te updaten
//        _mockAuth0Repository.Setup(x => x.UpdateUserEmailAsync(It.Is<UserDTO>(u => u.Email == "newemail@example.com")))
//                            .ReturnsAsync(updatedUserDto); // Nu retourneert het de UserDTO

//        // Mock de return van de UpdateUserCommandHandler
//        _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<UserDTO>())).Returns(updatedUserDto);

//        // Act
//        var command = new UpdateUserCommand
//        {
//            Id = userId,
//            User = updatedUserDto
//        };

//        var result = await _handler.Handle(command, CancellationToken.None);

//        // Assert
//        Assert.IsNotNull(result, "De return waarde van de handler is null.");
//        Assert.AreEqual(updatedUserDto.Email, result.Email, "De email is niet goed geüpdatet.");
//        Assert.AreEqual(originalUser.Name, result.Name, "De nickname zou niet veranderd moeten zijn.");
//    }


//}