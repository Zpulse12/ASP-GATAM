using Gatam.Application.CQRS;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.ControllerTest.TeamInvitation;

[TestClass]
public class AcceptTeamInvitationControllerTest
{
    private Mock<IMediator>? mediator;
    private InvitationTeamController? controller;

    [TestInitialize]
    public void Setup()
    {
        mediator = new Mock<IMediator>();
        controller = new InvitationTeamController(mediator.Object);
    }

    [TestMethod]
    public async Task AcceptInvitation_Test_Success()
    {
        // Arrange
        var jane = new Gatam.Domain.ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "JaneDoe",
            NormalizedUserName = "JANEDOE",
            Email = "jane.doe@example.com",
            NormalizedEmail = "JANE.DOE@EXAMPLE.COM",
            PasswordHash = "hashedpassword123",
            IsActive = false
        };

        var lauren = new Gatam.Domain.ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Lautje",
            NormalizedUserName = "LAUTJE",
            Email = "lautje.doe@example.com",
            NormalizedEmail = "LAUTJE.DOE@EXAMPLE.COM",
            PasswordHash = "hashedpassword123",
            IsActive = false
        };

        var GLOBALTESTTEAM = new Gatam.Domain.ApplicationTeam
        {
            Id = Guid.NewGuid().ToString(),
            TeamName = "TeamTest1",
            TeamCreatorId = jane.Id,
            IsDeleted = false,
            CreatedAt = DateTime.UnixEpoch,
        };

        var in1 = new Gatam.Domain.TeamInvitation
        {
            Id = Guid.NewGuid().ToString(),
            ApplicationTeamId = GLOBALTESTTEAM.Id,
            UserId = lauren.Id,
            isAccepted = true,
            CreatedAt = DateTime.UtcNow,
            ResponseDateTime = DateTime.UtcNow,
        };

        var command = new AcceptTeamInvitationCommand
        {
            _teaminvitationId = in1.Id,
            IsAccepted = true
        };

        // Simuleer het ontvangen van de uitnodiging via de Mediator
        var teamInvitationDTO = new Gatam.Application.CQRS.TeamInvitationDTO
        {
            Id = in1.Id,
            UserId = in1.UserId,
            CreatedAt = in1.CreatedAt,
            ResponseDateTime = in1.ResponseDateTime
        };

        // Setup van de mock om een lijst met het DTO-object terug te geven
        mediator.Setup(m => m.Send(It.IsAny<AcceptTeamInvitationCommand>(), default))
            .ReturnsAsync(new List<TeamInvitationDTO> { teamInvitationDTO });

        // Act
        var result = await controller.AcceptInvitation(in1.Id, command);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);

        // Aangezien het resultaat nu een lijst is, verifiëren dat de lijst niet leeg is
        var resultValue = okResult.Value as IEnumerable<TeamInvitationDTO>;
        Assert.IsNotNull(resultValue);
        Assert.AreEqual(1, resultValue.Count()); // Er moet precies 1 item zijn
        Assert.AreEqual(teamInvitationDTO, resultValue.First()); // Controleer of het juiste item is
    }
}
