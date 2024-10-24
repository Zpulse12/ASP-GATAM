using Gatam.Application.CQRS;
using Gatam.Domain;
using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;


namespace UnitTesting.CQRSTest.TeamInvitation
{
    [TestClass]
    public class AddToTeamCommandTest
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

            // Mock het TeamInvitationRepository om Create aan te roepen
            var teamInvitationRepositoryMock = new Mock<IGenericRepository<Gatam.Domain.TeamInvitation>>();
            mockUnitOfWork.Setup(u => u.TeamInvitationRepository).Returns(teamInvitationRepositoryMock.Object);
            teamInvitationRepositoryMock.Setup(r => r.Create(It.IsAny<Gatam.Domain.TeamInvitation>()));

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
            TeamInvitationDTO result = null;

            try
            {
                result = await handler.Handle(command, CancellationToken.None);
            }
            catch (NullReferenceException ex)
            {
                Assert.Fail($"Test failed due to a NullReferenceException: {ex.Message}");
            }

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(GLOBALTESTINVITATIONDTO.Id, result.Id);
            Assert.AreEqual(true, result.IsAccepted);
            mockUnitOfWork.Verify(u => u.TeamInvitationRepository.Create(It.IsAny<Gatam.Domain.TeamInvitation>()), Times.Once);
            mockUnitOfWork.Verify(u => u.commit(), Times.Once);
        }
    }
}