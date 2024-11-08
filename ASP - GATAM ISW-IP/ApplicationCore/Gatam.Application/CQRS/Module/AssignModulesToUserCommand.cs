using MediatR;

namespace Gatam.Application.CQRS.Module;

public class AssignModulesToUserCommand : IRequest<UserDTO>
{
    public string VolgerId { get; set; }
    public string ModuleId { get; set; }
}