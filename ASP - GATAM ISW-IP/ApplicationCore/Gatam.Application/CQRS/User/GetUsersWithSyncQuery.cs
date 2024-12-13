using MediatR;

namespace Gatam.Application.CQRS.User;

public class GetUsersWithSyncQuery : IRequest<List<UserDTO>> { }

public class GetUsersWithSyncQueryHandler : IRequestHandler<GetUsersWithSyncQuery, List<UserDTO>>
{
    private readonly IMediator _mediator;

    public GetUsersWithSyncQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<List<UserDTO>> Handle(GetUsersWithSyncQuery request, CancellationToken cancellationToken)
    {
        var auth0Users = await _mediator.Send(new GetAllAuth0UsersQuery());
        var syncedUsers = new List<UserDTO>();

        foreach (var auth0User in auth0Users)
        {
            var syncedUser = await _mediator.Send(new SyncUserCommand
            {
                Auth0User = auth0User
            }, cancellationToken);

            syncedUsers.Add(syncedUser);
        }

        return syncedUsers;
    }
}