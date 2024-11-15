using AutoMapper;
using FluentValidation;
using Gatam.Application.Extensions;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System.Diagnostics;


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
            RuleFor(u => u._user.PasswordHash)
                .NotNull().NotEmpty()
                .WithMessage("Wachtwoord moet ingevuld zijn");

        }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;
        private readonly EnvironmentWrapper _environmentWrapper;

        public CreateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository, EnvironmentWrapper environmentWrapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _auth0Repository = auth0Repository;
            _environmentWrapper = environmentWrapper;
        }
        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {


            //request._user.Name = AESProvider.Encrypt(request._user.Name, _environmentWrapper.KEY);
            //request._user.Surname = AESProvider.Encrypt(request._user.Surname, _environmentWrapper.KEY);
            //request._user.Username = AESProvider.Encrypt(request._user.Username, _environmentWrapper.KEY);
            //request._user.Email = AESProvider.Encrypt(request._user.Email, _environmentWrapper.KEY);
            //request._user.PhoneNumber = AESProvider.Encrypt(request._user.PhoneNumber, _environmentWrapper.KEY);


            var createUser = await _auth0Repository.CreateUserAsync(_mapper.Map<ApplicationUser>(request._user));
           if(createUser!=null)
            {

                UserDTO user = _mapper.Map<UserDTO>(createUser);
                if (request._user.RolesIds?.Any() == true && RoleMapper.Roles.TryGetValue("VOLGER", out var volgerRoleId))
                {
                    user.RolesIds = new List<string> { volgerRoleId };

                    await _auth0Repository.UpdateUserRoleAsync(user);
                }

                var appUser = _mapper.Map<UserDTO>(user);
                await _unitOfWork.UserRepository.Create(createUser);
                await _unitOfWork.Commit();

                return appUser;
            }

            return request._user;


        }
    }
}
