using FluentValidation;
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
    public class AddToTeamCommand : IRequest<TeamInvitation>
    {
        public required TeamInvitation _teamInvitation { get; set; }
    }
    public class AddToTeamCommandValidator: AbstractValidator<AddToTeamCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddToTeamCommandValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            //check id
            RuleFor(validationObject => validationObject._teamInvitation.Id).NotNull().WithMessage("ID mag niet null zijn.");
            RuleFor(validationObject => validationObject._teamInvitation.Id).NotEmpty().WithMessage("ID mag niet leeg zijn.");

            //user validation
            RuleFor(validationObject => validationObject._teamInvitation.applicationUser).NotNull().WithMessage("User mag niet null zijn");
            RuleFor(validationObject => validationObject._teamInvitation.applicationUser).NotEmpty().WithMessage("User mag niet leeg zijn");
            RuleFor(validationObject => validationObject._teamInvitation.UserId).NotNull().WithMessage("User ID mag niet null zijn");
            RuleFor(validationObject => validationObject._teamInvitation.UserId).NotEmpty().WithMessage("User ID mag niet leeg zijn");

            //team validation
            RuleFor(validationObject => validationObject._teamInvitation.ApplicationTeamId).NotNull().WithMessage("Team ID mag niet null zijn");
            RuleFor(validationObject => validationObject._teamInvitation.ApplicationTeamId).NotEmpty().WithMessage("Team ID mag niet leeg zijn");
            RuleFor(validationObject => validationObject._teamInvitation.applicationTeam).NotNull().WithMessage("Team mag niet null zijn");
            RuleFor(validationObject => validationObject._teamInvitation.applicationTeam).NotEmpty().WithMessage("Team mag niet leeg zijn");

            //Gebruiker zit al in een team
            RuleFor(validationObject => validationObject._teamInvitation).MustAsync(async (invitation, cancellationToken) =>
            {
                var existingInvitation = await _unitOfWork.TeamInvitationRepository.FindFirstAsync(x => x.UserId == invitation.UserId && x.isAccepted);
                return existingInvitation == null;
            }).WithMessage("De gebruiker kan niet worden toegevoegd omdat hij/zij al in een team zit met een geaccepteerde uitnodiging.");
        }
    }
    public class AddToTeamCommandHandler : IRequestHandler<AddToTeamCommand, TeamInvitation>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddToTeamCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<TeamInvitation> Handle(AddToTeamCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.TeamInvitationRepository.Create(request._teamInvitation);
            await _unitOfWork.commit();
            return request._teamInvitation;
        }

    }
}
