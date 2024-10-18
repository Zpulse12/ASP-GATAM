using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class DeleteTeamInvitationCommand: IRequest<bool>
        {
            public required string TeamInvitationId { get; set; }
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
