using AutoMapper;
using Gatam.Application.Interfaces;
using MediatR;
using FluentValidation;

namespace Gatam.Application.CQRS.InvitationTeam
{
    public class AcceptTeamInvitationCommand : IRequest<IEnumerable<TeamInvitationDTO>>
    {
        public required string _teaminvitationId { get; set; }
        public bool IsAccepted { get; set; }
    }

    public class AcceptTeamInvitationValidators : AbstractValidator<AcceptTeamInvitationCommand>
    {
        private readonly IUnitOfWork _uow;
        public AcceptTeamInvitationValidators(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x._teaminvitationId)
                .NotEmpty()
                .WithMessage("Team invitationId cannot be empty");
            RuleFor(x => x.IsAccepted)
                .NotNull()
                .WithMessage("IsAccepted cannot be null");
            RuleFor(x => x._teaminvitationId)
                .MustAsync(async (invitationId, cancellation) =>
                {
                    var invitation = await _uow.TeamInvitationRepository.FindById(invitationId);
                    if (invitation == null) return false;
                    var team = await _uow.TeamRepository.FindById(invitation.ApplicationTeamId);
                    return team != null;
                })
                .WithMessage("The team for this invitation does not exist");
        }
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
