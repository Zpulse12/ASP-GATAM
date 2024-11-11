using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.User;
using Gatam.Application.Extensions;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.CQRSTest.ApplicationUser;
[TestClass]
public class UpdateUserCommandTest
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IMapper> _mockMapper;
    private Mock<IManagementApi> _mockAuth0Repository;
    private UpdateUserCommandHandler _handler;



    [TestInitialize]
    public void Setup()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockAuth0Repository = new Mock<IManagementApi>();
        _handler = new UpdateUserCommandHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockAuth0Repository.Object);
    }

    [TestMethod]
    public async Task ShouldUpdateNickname_WhenNicknameChanges()
    {
        // Arrange
        var userId = "12345";
        var originalUser = new Gatam.Domain.ApplicationUser
        {
            Id = userId,
            UserName = "OldNickname", // Oud Nickname
            Email = "user@example.com",
            IsActive = true
        };

        var updatedUserDto = new UserDTO
        {
            Id = userId,
            Nickname = "NewNickname",  // Nieuwe Nickname
            Email = originalUser.Email,
            IsActive = true,
            RolesIds = new List<string> { RoleMapper.Roles["BEHEERDER"] }
        };

        //  ophalen van de originele gebruiker
        _mockUnitOfWork.Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                       .ReturnsAsync(originalUser);

        // Mock de Auth0 API interactie om de nickname te updaten
        _mockAuth0Repository.Setup(x => x.UpdateUserNicknameAsync(It.Is<UserDTO>(u => u.Nickname == "NewNickname")))
                            .ReturnsAsync(updatedUserDto);  // Retourneer de bijgewerkte UserDTO

        // Mock de return van de UpdateUserCommandHandler
        _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<UserDTO>())).Returns(updatedUserDto);

        // Act
        var command = new UpdateUserCommand
        {
            Id = userId,
            User = updatedUserDto
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result, "De return waarde van de handler is null.");
        Assert.AreEqual("NewNickname", result.Nickname, "De nickname is niet goed geüpdatet.");
        Assert.AreEqual(originalUser.Email, result.Email, "De email zou niet veranderd moeten zijn.");
    }


    [TestMethod]
    public async Task ShouldUpdateEmail_WhenEmailChanges()
    {
        // Arrange
        var userId = "12345";
        var originalUser = new Gatam.Domain.ApplicationUser
        {
            Id = userId,
            UserName = "OldNickname",
            Email = "user@example.com", // Oude Email
            IsActive = true
        };

        var updatedUserDto = new UserDTO
        {
            Id = userId,
            Nickname = originalUser.UserName,
            Email = "newemail@example.com",  // Nieuwe Email
            IsActive = true,
            RolesIds = new List<string> { RoleMapper.Roles["BEHEERDER"] },
        };

        //ophalen van de originele gebruiker
        _mockUnitOfWork.Setup(x => x.UserRepository.FindById(It.IsAny<string>()))
                       .ReturnsAsync(originalUser);

        // Mock de interactie met de Auth0 API om de email te updaten
        _mockAuth0Repository.Setup(x => x.UpdateUserEmailAsync(It.Is<UserDTO>(u => u.Email == "newemail@example.com")))
                            .ReturnsAsync(updatedUserDto); // Nu retourneert het de UserDTO

        // Mock de return van de UpdateUserCommandHandler
        _mockMapper.Setup(m => m.Map<UserDTO>(It.IsAny<UserDTO>())).Returns(updatedUserDto);

        // Act
        var command = new UpdateUserCommand
        {
            Id = userId,
            User = updatedUserDto
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result, "De return waarde van de handler is null.");
        Assert.AreEqual(updatedUserDto.Email, result.Email, "De email is niet goed geüpdatet.");
        Assert.AreEqual(originalUser.UserName, result.Nickname, "De nickname zou niet veranderd moeten zijn.");
    }



}