﻿using AutoMapper;
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
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(validationObject => validationObject._user.Email).NotNull().WithMessage("Email mag niet null zijn");
            RuleFor(validationObject => validationObject._user.Email).NotEmpty().WithMessage("Email mag niet leeg zijn");
            RuleFor(x => x._user.Email)
                .MustAsync(async (email, cancellation) =>
                {
                    var existingUser = await _unitOfWork.UserRepository.FindByProperty("Email",email);
                    return existingUser == null;
                })
                .WithMessage("Email is al in gebruik");

            RuleFor(x => x._user.UserName)
                .NotEmpty().WithMessage("Gebruikersnaam mag niet leeg zijn")
                .MinimumLength(3).WithMessage("Gebruikersnaam moet minimaal 3 tekens bevatten");
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
