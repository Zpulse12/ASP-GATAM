using Gatam.Application.CQRS;
using Gatam.Application.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace UnitTesting.CQRSTest.TeamInvitation
{
    [TestClass]
    public class DeleteTeamInvitationCommandTest
    {
        private Mock<IGenericRepository<Gatam.Domain.TeamInvitation>>? teamRepository;
        private DeleteTeamInvitationCommandHandler? commandHandler;
        private Mock<IValidator<DeleteTeamInvitationCommand>>? _validatorMock;


        [TestInitialize]
        public void Setup()
        {
            teamRepository = new Mock<IGenericRepository<Gatam.Domain.TeamInvitation>>();
            _validatorMock = new Mock<IValidator<DeleteTeamInvitationCommand>>();
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteTeamInvitationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            commandHandler = new DeleteTeamInvitationCommandHandler(teamRepository.Object,_validatorMock.Object);
        }

        [TestMethod]
        public async Task Handle_DeleteTeamInvitation_Success()
        {
            // Arrange
            string invitationId = Guid.NewGuid().ToString();
            var invitation = new Gatam.Domain.TeamInvitation { Id = invitationId, ApplicationTeamId = "TeamID", UserId = "userID" }; // Maak een nieuwe TeamInvitation aan

            teamRepository.Setup(repo => repo.FindById(invitationId))
                .ReturnsAsync(invitation); // Simuleer dat de uitnodiging gevonden is

            // Act
            var result = await commandHandler.Handle(new DeleteTeamInvitationCommand { TeamInvitationId = invitationId }, CancellationToken.None);

            // Assert
            Assert.IsTrue(result); // Verwacht dat de operatie succesvol is
            teamRepository.Verify(repo => repo.Delete(invitation), Times.Once); // Controleer of Delete eenmaal is aangeroepen
        }

        [TestMethod]
        public async Task Handle_DeleteTeamInvitation_NotFound()
        {
            // Arrange
            string invitationId = Guid.NewGuid().ToString();

            teamRepository.Setup(repo => repo.FindById(invitationId))
                .ReturnsAsync((Gatam.Domain.TeamInvitation)null); // Simuleer dat de uitnodiging niet gevonden is

            // Act
            var result = await commandHandler.Handle(new DeleteTeamInvitationCommand { TeamInvitationId = invitationId }, CancellationToken.None);

            // Assert
            Assert.IsFalse(result); // Verwacht dat de operatie mislukt
            teamRepository.Verify(repo => repo.Delete(It.IsAny<Gatam.Domain.TeamInvitation>()), Times.Never); // Controleer of Delete nooit is aangeroepen
        }
         [TestMethod]
        public async Task Handle_Should_Throw_ValidationException_When_Validation_Fails()
        {
            string invitationId = Guid.NewGuid().ToString();
            var validationFailure = new ValidationFailure("TeamInvitationId", "Invalid TeamInvitationId");
            var validationResult = new ValidationResult(new[] { validationFailure });
            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteTeamInvitationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult); 
            var command = new DeleteTeamInvitationCommand { TeamInvitationId = invitationId };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => commandHandler.Handle(command, CancellationToken.None));
        }

        [TestMethod]
        public async Task Handle_CannotDeleteAcceptedInvitation()
        {
            string invitationId = Guid.NewGuid().ToString();
            var invitation = new Gatam.Domain.TeamInvitation { Id = invitationId, ApplicationTeamId = "TeamID", UserId = "userID", isAccepted = true }; // Mock an accepted invitation

            teamRepository.Setup(repo => repo.FindById(invitationId))
                .ReturnsAsync(invitation); 
            var validationFailure = new ValidationFailure("TeamInvitationId", "Cannot delete an accepted invitation");
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteTeamInvitationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult); 

            var command = new DeleteTeamInvitationCommand { TeamInvitationId = invitationId };
            await Assert.ThrowsExceptionAsync<ValidationException>(() => commandHandler.Handle(command, CancellationToken.None));
        }
    }
    
}
