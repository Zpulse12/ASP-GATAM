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

namespace Gatam.Application.CQRS
{
    public class CreateUserCommand: IRequest<ApplicationUser>
    {
        public required ApplicationUser _user { get; set; }

    }
    public class CreateUserCommandValidator : AbstractValidator<ApplicationUser>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(validationObject => validationObject.Email).NotNull().NotEmpty().WithMessage("Email mag niet leeg zijn.");
        }
    }


    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            await _unitOfWork.UserRepository.Create(request._user);
            await _unitOfWork.commit();
            return request._user;
        }
    }
}
