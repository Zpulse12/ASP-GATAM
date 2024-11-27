using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.RolesDTO;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User.Roles
{
    public class UpdateUserRoleCommand : IRequest<UserDTO>
    {
        public string Id { get; set; }
        public required RolesDTO Roles { get; set; }
    }

    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateUserRoleCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID cannot be empty")
                .MustAsync(async (id, cancellation) =>
                {
                    var user = await _uow.UserRepository.FindById(id);
                    return user != null;
                }).WithMessage("User does not exist");

            RuleFor(x => x.Roles)
                .NotNull().WithMessage("Roles object cannot be null");

            RuleFor(x => x.Roles.Roles)
                .NotEmpty().WithMessage("At least one role must be assigned")
                .NotNull().WithMessage("Roles list cannot be null")
                .Must(roles => roles.Distinct().Count() == roles.Count)
                .WithMessage("Duplicate roles found in request")
                .Must(roles => roles.All(role => RoleMapper.Roles.Values.Any(r => r.Id == role)))
                .WithMessage("One or more roles are invalid");

            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var user = await _uow.UserRepository.FindById(command.Id);
                    if (user == null) return true;
                    var newRoles = command.Roles.Roles.Except(user.UserRoles.Select(ur => ur.RoleId));
                    return newRoles.Any();
                }).WithMessage("These roles are already assigned to the user");
        }
    }
    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserDTO>
    {
        private readonly IManagementApi _auth0Repository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateUserRoleCommandHandler(IManagementApi auth0Repository, IUnitOfWork uow, IMapper mapper)
        {
            _auth0Repository = auth0Repository;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ApplicationUser user = await _uow.UserRepository.FindById(request.Id);
                foreach (var role in request.Roles.Roles)
                {
                    user.UserRoles.Add(new UserRole { RoleId = role, UserId = user.Id });
                }
                await _uow.UserRepository.Update(user);
                await _uow.Commit();
                Result<bool> result = await _auth0Repository.UpdateUserRoleAsync(request.Id, request.Roles);
                if (result.Success)
                {
                    return _mapper.Map<UserDTO>(user);
                }
                else {
                    try
                    {
                        var rolesToRemove = user.UserRoles.Where(ur => request.Roles.Roles.Contains(ur.RoleId)).ToList();
                        foreach (var role in rolesToRemove)
                        {
                            user.UserRoles.Remove(role);
                        }
                        await _uow.UserRepository.Update(user);
                        await _uow.Commit();
                        throw result.Exception;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
