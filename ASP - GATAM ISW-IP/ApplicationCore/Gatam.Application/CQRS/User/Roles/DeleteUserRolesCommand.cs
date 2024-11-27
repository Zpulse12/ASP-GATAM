using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;


namespace Gatam.Application.CQRS.User.Roles
{
    public class DeleteUserRolesCommand : IRequest<UserDTO>
    {
        public RolesDTO Roles { get; set; }
        public string UserId { get; set; }
    }
    public class DeleteUserRolesCommandValidator : AbstractValidator<DeleteUserRolesCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly IManagementApi _managementApi;
        public DeleteUserRolesCommandValidator(IManagementApi managementApi, IUnitOfWork uow) 
        { 
            _uow = uow;
            _managementApi = managementApi;
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID cannot be empty");
            RuleFor(x => x.Roles.Roles)
                .NotEmpty().WithMessage("Roles cannot be empty");
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("User ID cannot be null");
            RuleFor(x => x.Roles.Roles)
                .NotNull().WithMessage("Roles cannot be null");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await _uow.UserRepository.FindById(userId);
                    return user != null;
                })
                .WithMessage("The user doesnt exist");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await _managementApi.GetUserByIdAsync(userId);
                    return user != null;
                })
                .WithMessage("The user doesnt exist");
        }
    }
    public class DeleteUserRolesCommandHandler : IRequestHandler<DeleteUserRolesCommand, UserDTO>
    {
        private readonly IManagementApi _managementApi;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public DeleteUserRolesCommandHandler(IManagementApi managementApi, IUnitOfWork uow, IMapper mapper)
        {
            _managementApi = managementApi;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(DeleteUserRolesCommand request, CancellationToken cancellationToken)
        {
            Result<bool> auth0Result = await _managementApi.DeleteUserRoles(request.UserId, request.Roles);
            if (!auth0Result.Success)
            {
                throw auth0Result.Exception ?? new Exception("Failed to delete roles in Auth0");
            }

            var user = await _uow.UserRepository.GetUserWithRoles(request.UserId);
            
            var rolesToRemove = user.UserRoles
                .Where(ur => request.Roles.Roles.Contains(ur.RoleId))
                .ToList();

            foreach (var roleToRemove in rolesToRemove)
            {
                _uow.UserRepository.RemoveUserRole(roleToRemove);
            }

            await _uow.Commit();

            return _mapper.Map<UserDTO>(user);
        }
    }

}
