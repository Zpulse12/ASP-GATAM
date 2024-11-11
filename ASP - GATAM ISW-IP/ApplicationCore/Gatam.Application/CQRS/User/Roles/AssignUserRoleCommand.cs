using FluentValidation;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User.Roles
{
    public class AssignUserRoleCommand : IRequest<UserDTO>
    {
        public string Id { get; set; }
        public required UserDTO User { get; set; }
    }

    public class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand>
    {
        public AssignUserRoleCommandValidator()
        {
            RuleFor(x => x.User.Id)
                .NotEmpty().WithMessage("Id cannot be empty")
                .Equal(x => x.Id)
                .WithMessage("Id does not equal given userId");
            RuleFor(x => x.User.RolesIds)
                .NotEmpty().WithMessage("Role cannot be empty")
                .NotNull().WithMessage("Role cannot be null");
        }
    }
    public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, UserDTO>
    {
        private readonly IManagementApi _auth0Repository;

        public AssignUserRoleCommandHandler(IManagementApi auth0Repository)
        {
            _auth0Repository = auth0Repository;
        }

        public async Task<UserDTO> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            await _auth0Repository.UpdateUserRoleAsync(request.User);
            return request.User;
        }
    }
}
