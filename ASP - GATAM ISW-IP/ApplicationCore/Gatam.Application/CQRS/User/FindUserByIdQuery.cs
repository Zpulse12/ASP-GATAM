using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User;

public class FindUserByIdQuery : IRequest<UserDTO>
{
    public string Auth0UserId { get; set; }
    
    public FindUserByIdQuery(string auth0UserId)
    {
        Auth0UserId = auth0UserId;
    }
}

public class FindUserByIdQueryHandler : IRequestHandler<FindUserByIdQuery, UserDTO>
{
    private readonly IManagementApi _managementApi;

    public FindUserByIdQueryHandler(IManagementApi managementApi)
    {
        _managementApi = managementApi;
    }

    public async Task<UserDTO> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _managementApi.GetUserByIdAsync(request.Auth0UserId);
    }
}
