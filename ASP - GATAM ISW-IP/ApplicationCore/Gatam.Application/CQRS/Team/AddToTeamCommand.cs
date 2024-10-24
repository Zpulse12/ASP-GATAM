using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Team
{
    public class AddToTeamCommand : IRequest<TeamInvitationDTO>
    {
        public required TeamInvitationDTO _teamInvitation { get; set; }
    }
    public class AddToTeamCommandValidator : AbstractValidator<AddToTeamCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToTeamCommandValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            //check id
            RuleFor(validationObject => validationObject._teamInvitation.Id).NotNull().WithMessage("ID mag niet null zijn.");
            RuleFor(validationObject => validationObject._teamInvitation.Id).NotEmpty().WithMessage("ID mag niet leeg zijn.");

            //user validation
            RuleFor(validationObject => validationObject._teamInvitation.UserId).NotNull().WithMessage("User ID mag niet null zijn");
            RuleFor(validationObject => validationObject._teamInvitation.UserId).NotEmpty().WithMessage("User ID mag niet leeg zijn");

            //team validation
            RuleFor(validationObject => validationObject._teamInvitation.ApplicationTeamId).NotNull().WithMessage("Team ID mag niet null zijn");
            RuleFor(validationObject => validationObject._teamInvitation.ApplicationTeamId).NotEmpty().WithMessage("Team ID mag niet leeg zijn");

            //Gebruiker zit al in een team
            RuleFor(validationObject => validationObject._teamInvitation).MustAsync(async (invitation, cancellationToken) =>
            {
                var existingInvitation = await _unitOfWork.TeamInvitationRepository.FindFirstAsync(x => x.UserId == invitation.UserId && x.isAccepted);
                return existingInvitation == null;
            }).WithMessage("De gebruiker kan niet worden toegevoegd omdat hij/zij al in een team zit.");
        }
    }
    public class AddToTeamCommandHandler : IRequestHandler<AddToTeamCommand, TeamInvitationDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddToTeamCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<TeamInvitationDTO> Handle(AddToTeamCommand request, CancellationToken cancellationToken)
        {
            var teamInvitation = new TeamInvitation()
            {
                Id = request._teamInvitation.Id,
                ApplicationTeamId = request._teamInvitation.ApplicationTeamId,
                UserId = request._teamInvitation.UserId,
                CreatedAt = DateTime.UtcNow,
                ResponseDateTime = DateTime.UtcNow,
                isAccepted = true
            };

            await _unitOfWork.TeamInvitationRepository.Create(teamInvitation);
            await _unitOfWork.commit();
            return new TeamInvitationDTO()
            {
                Id = teamInvitation.Id,
                ApplicationTeamId = teamInvitation.ApplicationTeamId,
                UserId = teamInvitation.UserId,
                CreatedAt = teamInvitation.CreatedAt,
                ResponseDateTime = teamInvitation.ResponseDateTime,
                IsAccepted = teamInvitation.isAccepted
            };
        }

    }
}
