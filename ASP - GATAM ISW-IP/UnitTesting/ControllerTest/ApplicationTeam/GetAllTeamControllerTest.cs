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

namespace UnitTesting.ControllerTest.ApplicationTeam
{
    [TestClass]
    public class GetAllTeamInvitationControllerTest
    {
        private Mock<IMediator>? mediator;
        private TeamController? controller;  // Aangepast naar TeamController

        private readonly Gatam.Domain.ApplicationUser GLOBALTESTUSER = new Gatam.Domain.ApplicationUser()
        {
            UserName = "admin",
            Email = "admin@app.com"
        };

        [TestInitialize]
        public void Setup()
        {
            mediator = new Mock<IMediator>();
            controller = new TeamController(mediator.Object);
        }

        [TestMethod]
        public async Task GetTeams_ReturnsOkResult_WithTeams()
        {
            var teams = new List<Gatam.Domain.ApplicationTeam>
            {
                new Gatam.Domain.ApplicationTeam
                {
                    Id = Guid.NewGuid().ToString(),
                    TeamName = "test team",
                    TeamCreatorId = GLOBALTESTUSER.Id,
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow
                }
            };

            mediator.Setup(m => m.Send(It.IsAny<GetAllTeamsQuery>(), default))
                    .ReturnsAsync(teams);

            var result = await controller.GetTeams();  

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnedTeams = okResult.Value as List<Gatam.Domain.ApplicationTeam>;
            Assert.IsNotNull(returnedTeams);
            Assert.AreEqual(teams.Count, returnedTeams.Count);  // Verifieer dat het aantal teams overeenkomt
            Assert.AreEqual(teams[0].TeamName, returnedTeams[0].TeamName);
        }
    }
}
