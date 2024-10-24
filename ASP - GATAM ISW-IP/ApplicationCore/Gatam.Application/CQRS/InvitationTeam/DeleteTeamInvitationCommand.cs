using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Gatam.Application.CQRS.InvitationTeam
{
    public class DeleteTeamInvitationCommand : IRequest<bool>
    {
        public required string TeamInvitationId { get; set; }
    }

    public class DeleteTeamInvitationValidators : AbstractValidator<DeleteTeamInvitationCommand>
    {
        private readonly IUnitOfWork _uow;

        public DeleteTeamInvitationValidators(IUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x.TeamInvitationId).NotEmpty();
            RuleFor(x => x.TeamInvitationId)
                .MustAsync(async (invitationId, cancellationToken) =>
                {
                    var invitation = await _uow.TeamInvitationRepository.FindById(invitationId);
                    return invitation != null;
                })
                .WithMessage("The invitation does not exist");
            RuleFor(x => x.TeamInvitationId)
                .MustAsync(async (invitationId, cancellationToken) =>
                {
                    var invitation = await _uow.TeamInvitationRepository.FindById(invitationId);
                    if (invitation == null) return false;

                    return !invitation.isAccepted;
                })
                .WithMessage("Cannot delete an invitation that has already been accepted");

        }
    }

    public class DeleteTeamInvitationCommandHandler(IGenericRepository<TeamInvitation> teamRepository) : IRequestHandler<DeleteTeamInvitationCommand, bool>
    {
        public async Task<bool> Handle(DeleteTeamInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await teamRepository.FindById(request.TeamInvitationId);
            if (invitation == null) return false;
            await teamRepository.Delete(invitation);
            return true;
        }
    }
}
