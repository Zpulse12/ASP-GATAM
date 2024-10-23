using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;

namespace UnitTesting.CQRSTest.TeamInvitation
{
    [TestClass]
    public class AcceptInvitationCommandTest
    {
        private Mock<IUnitOfWork> mockUnitOfWork;
        private Mock<IMapper> mockMapper;
        private IRequestHandler<AcceptTeamInvitationCommand, IEnumerable<TeamInvitationDTO>> handler;
        private Gatam.Domain.TeamInvitation GLOBALTESTINVITATION;

        [TestInitialize]
        public void Setup()
        {
            // Setup een test teamuitnodiging
            GLOBALTESTINVITATION = new Gatam.Domain.TeamInvitation()
            {
                Id = Guid.NewGuid().ToString(),
                ApplicationTeamId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                isAccepted = false,
                CreatedAt = DateTime.UtcNow,
                ResponseDateTime = DateTime.UtcNow
            };

            mockUnitOfWork = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();

            handler = new AcceptTeamInvitationCommandHandler(mockUnitOfWork.Object, mockMapper.Object);
        }

        [TestMethod]
        public async Task Handle_AcceptsInvitation_WhenInvitationExists()
        {
            // Arrange
            var command = new AcceptTeamInvitationCommand
            {
                _teaminvitationId = GLOBALTESTINVITATION.Id,
                IsAccepted = true
            };

            mockUnitOfWork.Setup(u => u.TeamInvitationRepository.FindById(command._teaminvitationId))
                .ReturnsAsync(GLOBALTESTINVITATION);

            mockMapper.Setup(m => m.Map<TeamInvitationDTO>(GLOBALTESTINVITATION))
                .Returns(new TeamInvitationDTO
                {
                    Id = GLOBALTESTINVITATION.Id,
                    IsAccepted = command.IsAccepted
                });

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(true, result.First().IsAccepted);
            Assert.AreEqual(GLOBALTESTINVITATION.Id, result.First().Id);
            mockUnitOfWork.Verify(u => u.TeamInvitationRepository.Update(GLOBALTESTINVITATION), Times.Once);
            mockUnitOfWork.Verify(u => u.commit(), Times.Once);
        }

        [TestMethod]
        public async Task Handle_ReturnsNull_WhenInvitationDoesNotExist()
        {
            // Arrange
            var command = new AcceptTeamInvitationCommand
            {
                _teaminvitationId = Guid.NewGuid().ToString(),
                IsAccepted = true
            };

            mockUnitOfWork.Setup(u => u.TeamInvitationRepository.FindById(command._teaminvitationId))
                .ReturnsAsync((Gatam.Domain.TeamInvitation)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsNull(result);
            mockUnitOfWork.Verify(u => u.TeamInvitationRepository.Update(It.IsAny<Gatam.Domain.TeamInvitation>()), Times.Never);
            mockUnitOfWork.Verify(u => u.commit(), Times.Never);
        }
    }
    
}
