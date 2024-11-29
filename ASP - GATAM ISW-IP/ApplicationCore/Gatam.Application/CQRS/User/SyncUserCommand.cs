using AutoMapper;
using Gatam.Application.CQRS;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

public class SyncUserCommand : IRequest<UserDTO>
{
    public required UserDTO Auth0User { get; set; }
}

public class SyncUserCommandHandler : IRequestHandler<SyncUserCommand, UserDTO>
{
    private readonly IManagementApi _managementApi;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SyncUserCommandHandler(IManagementApi managementApi, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _managementApi = managementApi;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDTO> Handle(SyncUserCommand request, CancellationToken cancellationToken)
    {
        var localUser = await _unitOfWork.UserRepository.FindById(request.Auth0User.Id);

        if (localUser == null)
        {
            localUser = _mapper.Map<ApplicationUser>(request.Auth0User);
            localUser.Id = request.Auth0User.Id;
            localUser.IsActive = true;
            await _unitOfWork.UserRepository.Create(localUser);
        }
        else
        {
            _mapper.Map(request.Auth0User, localUser);
            await _unitOfWork.UserRepository.Update(localUser);
        }

        var auth0Roles = await _managementApi.GetRolesByUserId(request.Auth0User.Id);

        if (localUser.UserRoles == null)
        {
            localUser.UserRoles = new List<UserRole>();
        }
        else
        {
            localUser.UserRoles.Clear();
        }

        foreach (var roleName in auth0Roles)
        {
            var role = RoleMapper.Roles.FirstOrDefault(r => r.Value.Name == roleName).Value;
            localUser.UserRoles.Add(new UserRole 
            { 
                UserId = localUser.Id,
                RoleId = role.Id
            });
        }

        await _unitOfWork.Commit();
        return _mapper.Map<UserDTO>(localUser);
    }
} 