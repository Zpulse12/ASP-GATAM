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
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(validationObject => validationObject._user.Email).NotNull().WithMessage("Email cannot be null");
            RuleFor(validationObject => validationObject._user.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(x => x._user.Email)
                .MustAsync(async (email, cancellation) =>
                {
                    var existingUser = await _unitOfWork.UserRepository.FindByProperty("Email",email);
                    return existingUser == null;
                })
                .WithMessage("Email is already in use");

            RuleFor(x => x._user.UserName)
                .NotEmpty().WithMessage("Username cannot be empty")
                .MinimumLength(3).WithMessage("username must contain minimal 3 letters");
            RuleFor(x => x._user.PasswordHash)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;

        public CreateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _auth0Repository = auth0Repository;
        }
        public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            var createUser = await _auth0Repository.CreateUserAsync(request._user);
            return _mapper.Map<ApplicationUser>(createUser);


        }
    }
}
