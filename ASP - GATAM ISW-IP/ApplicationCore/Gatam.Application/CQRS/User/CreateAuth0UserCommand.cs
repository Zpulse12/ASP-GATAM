using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Application.CQRS.DTOS.UsersDTO;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;


namespace Gatam.Application.CQRS.User
{
    public class CreateAuth0UserCommand : IRequest<UserDTO>
    {
        public required CreateUserDTO _user { get; set; }
    }
    public class CreateAuth0UserCommandValidator : AbstractValidator<CreateAuth0UserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateAuth0UserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(u => u._user.Name)
                .NotNull().NotEmpty()
                .WithMessage("Voornaam mag niet leeg zijn");
            RuleFor(u => u._user.Name)
                .MaximumLength(50)
                .WithMessage("Voornaam is te lang");
            RuleFor(u => u._user.Surname)
                .NotNull().NotEmpty()
                .WithMessage("Achternaam mag niet leeg zijn");
            RuleFor(u => u._user.Surname)
                .MaximumLength(50)
                .WithMessage("Achternaam is te lang");
            RuleFor(u => u._user.Username)
                .NotNull().NotEmpty()
                .WithMessage("Username Mag niet leeg zijn");
            RuleFor(u => u._user.Email)
                .NotNull().NotEmpty()
                .WithMessage("Email moet ingevuld zijn");
            RuleFor(u => u._user.Email)
                .EmailAddress()
                .WithMessage("Email moet een geldig emailadres zijn");
            RuleFor(u => u._user.PhoneNumber)
                .NotNull().NotEmpty()
                .WithMessage("Gsm-nummer moet ingevuld zijn");
        }
    }
    public class CreateAuth0UserCommandHandler : IRequestHandler<CreateAuth0UserCommand, UserDTO>
    {
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;
        public CreateAuth0UserCommandHandler(IMapper mapper, IManagementApi auth0Repository)
        {
            _mapper = mapper;
            _auth0Repository = auth0Repository;
        }
        public async Task<UserDTO> Handle(CreateAuth0UserCommand request, CancellationToken cancellationToken)
        {
            Result<CreateUserDTO> createUser = await _auth0Repository.CreateUserAsync(_mapper.Map<CreateUserDTO>(request._user));

            if (!createUser.Success)
            {
                throw new InvalidOperationException($"Failed to create user: {createUser.Exception?.Message}", createUser.Exception);
            }

            var user = _mapper.Map<ApplicationUser>(createUser.Value);

            user.UserRoles = new List<UserRole>();

            var volgerRole = RoleMapper.Roles[CustomRoles.VOLGER];
            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = volgerRole.Id
            });

            var rolesToUpdate = new List<string> { volgerRole.Id };
            await _auth0Repository.UpdateUserRoleAsync(user.Id, new RolesDTO() { Roles = rolesToUpdate });

            return _mapper.Map<UserDTO>(user);
        }
    }

}
