using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Gatam.Application.CQRS;

public class UpdateUserCommand:IRequest<UserDTO>
{
    public string Id { get; set; }
    public required UserDTO User { get; set; }

}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(x => x.User.Username)
            .NotEmpty().WithMessage("Username cannot be empty")
            .MustAsync(async (userCommand, username, cancellationToken) =>
            {
                var existingUser = await _unitOfWork.UserRepository.FindByProperty("UserName", username);
                return existingUser == null || existingUser.Id == userCommand.Id;
            }).WithMessage("Username already exists.");

        RuleFor(x => x.User.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(async (userCommand, email, cancellationToken) =>
            {
                var existingUser = await _unitOfWork.UserRepository.FindByProperty("Email", email);
                return existingUser == null || existingUser.Id == userCommand.Id;
            }).WithMessage("Email already exists.");
        RuleFor(x => x.User.Id)
            .NotEmpty().WithMessage("Id cannot be empty")
            .Equal(x => x.Id)
            .WithMessage("Id does not equal given userId");
        RuleFor(x => x.User.RolesIds)
            .NotEmpty().WithMessage("Role cannot be empty")
            .NotNull().WithMessage("Role cannot be null");



    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IManagementApi _auth0Repository;

    public UpdateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository)
    {
        _uow = uow;
        _mapper = mapper;
        _auth0Repository = auth0Repository;
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
     
        var updatedPerson = await _auth0Repository.UpdateUserAsync(request.Id, request.User);

        if (request.User.RolesIds != null && request.User.RolesIds.Any())
        {
            var roleIds = request.User.RolesIds
                 .Select(roleName => RoleMapper.Roles.ContainsKey(roleName) ? RoleMapper.Roles[roleName] : null)
                 .Where(roleId => roleId != null)
                 .ToList();

            // Check if there are any valid role IDs to update / validation
            if (roleIds.Any())
            {
                await _auth0Repository.UpdateUserRoleAsync(request.User);
            }
        }

        return _mapper.Map<UserDTO>(updatedPerson);
    }

    
}
