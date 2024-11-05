using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.CQRS.Team;
using Gatam.Application.Interfaces;
using Gatam.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting.ControllerTest.TeamInvitation
{
    [TestClass]
    internal class AddToTeamCommandTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private IRequestHandler<AddToTeamCommand, TeamInvitationDTO> handler;

        private TeamInvitationDTO GLOBALTESTINVITATIONDTO;

        [TestInitialize]
        public void Setup()
        {
            GLOBALTESTINVITATIONDTO = new TeamInvitationDTO()
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationTeamId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                IsAccepted = false,
                CreatedAt = DateTime.UtcNow,
                ResponseDateTime = DateTime.UtcNow
            };

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();

            handler = new AddToTeamCommandHandler(mockUnitOfWork.Object);
        }

        [TestMethod]
        public async Task Handle_AddsTeamInvitation_WhenValidCommand()
        {
            // Arrange
            var command = new AddToTeamCommand
            {
                _teamInvitation = GLOBALTESTINVITATIONDTO
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(GLOBALTESTINVITATIONDTO.Id, result.Id);
            Assert.AreEqual(true, result.IsAccepted);
            mockUnitOfWork.Verify(u => u.TeamInvitationRepository.Create(It.IsAny<Gatam.Domain.TeamInvitation>()), Times.Once);
            mockUnitOfWork.Verify(u => u.commit(), Times.Once);
        }
    }
}