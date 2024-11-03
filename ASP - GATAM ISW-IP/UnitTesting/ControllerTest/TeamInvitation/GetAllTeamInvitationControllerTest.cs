using Gatam.Application.CQRS;
using Gatam.Domain;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Application.CQRS.User;

namespace UnitTesting.ControllerTest.TeamInvitation
{
    [TestClass]
    public class TeamInvitationControllerTest
    {
        private Mock<IMediator>? mediator;
        private InvitationTeamController? controller;

        private Gatam.Domain.ApplicationUser john = new Gatam.Domain.ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "JohnDoe",
            NormalizedUserName = "JOHNDOE",
            Email = "john.doe@example.com",
            NormalizedEmail = "JOHN.DOE@EXAMPLE.COM",
            PasswordHash = "hashedpassword123"
        };

        private Gatam.Domain.ApplicationUser jane = new Gatam.Domain.ApplicationUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "JaneDoe",
            NormalizedUserName = "JANEDOE",
            Email = "jane.doe@example.com",
            NormalizedEmail = "JANE.DOE@EXAMPLE.COM",
            PasswordHash = "hashedpassword123"
        };

        private Gatam.Domain.ApplicationTeam GLOBALTESTTEAM = new Gatam.Domain.ApplicationTeam()
        {
            Id = Guid.NewGuid().ToString(),
            TeamName = "test team",
            TeamCreatorId = "admin-id",
            IsDeleted = false,
            CreatedAt = DateTime.UnixEpoch,
        };

        [TestInitialize]
        public void Setup()
        {
            mediator = new Mock<IMediator>();
            controller = new InvitationTeamController(mediator.Object);
        }

        [TestMethod]
        public async Task GetTeamInvitations_ReturnsOkResult_WithInvitations()
        {
            var invitations = new List<Gatam.Domain.TeamInvitation>()
            {
                new Gatam.Domain.TeamInvitation
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationTeamId = GLOBALTESTTEAM.Id,
                    UserId = john.Id,
                    isAccepted = true,
                    CreatedAt = DateTime.UtcNow,
                    ResponseDateTime = DateTime.UtcNow
                },
                new Gatam.Domain.TeamInvitation
                {
                    Id = Guid.NewGuid().ToString(),
                    ApplicationTeamId = GLOBALTESTTEAM.Id,
                    UserId = jane.Id,
                    isAccepted = false,
                    CreatedAt = DateTime.UtcNow,
                    ResponseDateTime = DateTime.UtcNow
                }
            };

            mediator.Setup(m => m.Send(It.IsAny<GetAllInvitationsQuery>(), default))
                    .ReturnsAsync(invitations);

            var result = await controller.GetInvitationsTeams();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(invitations, okResult.Value);
        }
    }
}
