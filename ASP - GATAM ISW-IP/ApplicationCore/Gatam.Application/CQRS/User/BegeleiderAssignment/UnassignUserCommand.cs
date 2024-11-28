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
    public class UnassignUserCommand : IRequest<UserDTO>
    {
        public required string VolgerId { get; set; }
        public UserDTO User { get; set; }
    }

    public class UnassignUserCommandValidator : AbstractValidator<UnassignUserCommand>
    {
        public UnassignUserCommandValidator()
        {
            RuleFor(x => x.VolgerId)
                .NotEmpty().WithMessage("VolgerId mag niet leeg zijn");
            RuleFor(x => x.User).NotNull().WithMessage("Gebruiker mag niet null zijn")
                .NotEmpty().WithMessage("Gebruiker mag niet leeg zijn");
            
        }
    }

    public class UnassignUserCommandHandler : IRequestHandler<UnassignUserCommand, UserDTO>
    {
        private readonly IUnitOfWork _uow;

        public UnassignUserCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserDTO> Handle(UnassignUserCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _uow.UserRepository.FindById(request.VolgerId);
            userEntity.BegeleiderId = null;
            await _uow.UserRepository.Update(userEntity);
            await _uow.Commit();
            return request.User;
        }
    }
}
