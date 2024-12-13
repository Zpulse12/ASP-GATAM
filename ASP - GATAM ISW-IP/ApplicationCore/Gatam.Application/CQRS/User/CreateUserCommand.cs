using AutoMapper;
using FluentValidation;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;


namespace Gatam.Application.CQRS.User
{
    public class CreateUserCommand: IRequest<UserDTO>
    {
        public required UserDTO _user { get; set; }
    }
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
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
                .WithMessage("UserNam Mag niet leeg zijn");
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
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;
        public CreateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }
        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<ApplicationUser>(request._user);
            user.Id = request._user.Id;
            user.UserRoles = new List<UserRole>();
            var volgerRole = RoleMapper.Roles[CustomRoles.VOLGER];
            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = volgerRole.Id
            });
            await _unitOfWork.UserRepository.Create(user);
            await _unitOfWork.Commit();
            return _mapper.Map<UserDTO>(user);
        }
    }
}
