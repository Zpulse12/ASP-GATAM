using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class UnassignUserCommand : IRequest<ApplicationUser>
    {
        public required string VolgerId { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class UnassignUserCommandValidator : AbstractValidator<UnassignUserCommand>
    {
        public UnassignUserCommandValidator()
        {
            RuleFor(x => x.VolgerId)
                .NotEmpty().WithMessage("VolgerId mag niet leeg zijn");
            RuleFor(x => x.User).NotNull().WithMessage("Geberuiker mag niet null zijn")
                .NotEmpty().WithMessage("Gebruiker mag niet leeg zijn");
            
        }
    }

    public class UnassignUserCommandHandler : IRequestHandler<UnassignUserCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _uow;

        public UnassignUserCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
        }

        public async Task<ApplicationUser> Handle(UnassignUserCommand request, CancellationToken cancellationToken)
        {
            request.User.BegeleiderId = null;
            await _uow.UserRepository.Update(request.User);
            await _uow.Commit();
            return request.User;
        }
    }
}
