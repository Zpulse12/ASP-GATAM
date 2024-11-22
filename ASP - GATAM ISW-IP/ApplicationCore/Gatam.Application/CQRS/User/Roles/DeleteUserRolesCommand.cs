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
            Result<bool> result = await _managementApi.DeleteUserRoles(request.UserId, request.Roles);
            if (result.Success)
            {
                try
                {
                    ApplicationUser user = await _uow.UserRepository.FindById(request.UserId);
                    user.RolesIds.RemoveAll(item => request.Roles.Roles.Contains(item));
                    await _uow.UserRepository.Update(user);
                    await _uow.Commit();
                    return _mapper.Map<UserDTO>(user);
                }
                catch (Exception ex) {
                    throw ex;
                }
            } else
            {
                throw result.Exception;
            }
        }
    }

}
