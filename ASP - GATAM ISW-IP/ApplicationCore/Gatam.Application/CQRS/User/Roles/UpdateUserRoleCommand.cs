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
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be empty")
                .Equal(x => x.Id)
                .WithMessage("Id does not equal given userId");
            RuleFor(x => x.Roles.Roles)
                .NotEmpty().WithMessage("Role cannot be empty")
                .NotNull().WithMessage("Role cannot be null");
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
            Result<bool> result = await _auth0Repository.UpdateUserRoleAsync(request.Id, request.Roles);
            if (result.Success) {
                try
                {
                    ApplicationUser user = await _uow.UserRepository.FindById(request.Id);
                    user.RolesIds.AddRange(request.Roles.Roles.Except(user.RolesIds));
                    await _uow.UserRepository.Update(user);
                    await _uow.Commit();
                    return _mapper.Map<UserDTO>(user);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw result.Exception;
            }
        }
    }
}
