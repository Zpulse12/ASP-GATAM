using AutoMapper;
using Gatam.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class AcceptTeamInvitationCommand : IRequest<IEnumerable<TeamInvitationDTO>>
    {
        public required string _teaminvitationId { get; set; }
        public bool IsAccepted { get; set; }
    }

    public class AcceptTeamInvitationCommandHandler : IRequestHandler<AcceptTeamInvitationCommand, IEnumerable<TeamInvitationDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public AcceptTeamInvitationCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<TeamInvitationDTO>> Handle(AcceptTeamInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await uow.TeamInvitationRepository.FindById(request._teaminvitationId);
            if (invitation == null)
            {
                return null;
            }
            invitation.isAccepted = request.IsAccepted;
            await uow.TeamInvitationRepository.Update(invitation);
            await uow.commit();

            var teamInvitationDTO = mapper.Map<TeamInvitationDTO>(invitation);
            return new List<TeamInvitationDTO> { teamInvitationDTO };
        }
    }

}
