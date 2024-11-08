using MediatR;

namespace Gatam.Application.CQRS.Module;

public class AssignModulesToUserCommand : IRequest<UserDTO>
{
    public string VolgerId { get; set; }
    public string ModuleId { get; set; }
}

public class AssignModulesToUserCommandHandler : IRequestHandler<AssignModulesToUserCommand, UserDTO>
{
    public Task<UserDTO> Handle(AssignModulesToUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}