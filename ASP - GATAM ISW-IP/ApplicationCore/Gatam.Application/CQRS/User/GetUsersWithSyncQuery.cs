using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User;

public class GetUsersWithSyncQuery : IRequest<List<UserDTO>> { }

public class GetUsersWithSyncQueryHandler : IRequestHandler<GetUsersWithSyncQuery, List<UserDTO>>
{
    private readonly IMediator _mediator;
    public GetUsersWithSyncQueryHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
    }

    public async Task<List<UserDTO>> Handle(GetUsersWithSyncQuery request, CancellationToken cancellationToken)
    {
        var auth0Users = await _mediator.Send(new GetAllUsersQuery());

        var usersWithLocalStatus = new List<UserDTO>();

        foreach (var auth0User in auth0Users)
        {
            var localUser = await _mediator.Send(new FindUserByIdQuery(auth0User.Id));

            if (localUser == null)
            {
                await _mediator.Send(new CreateUserCommand { User = new ApplicationUser
                {
                    Id = auth0User.Id,
                    IsActive = true,
                } });
                auth0User.IsActive = true;
            }
            else
            {
                auth0User.IsActive = localUser.IsActive;
            }

            usersWithLocalStatus.Add(auth0User);
        }

        return usersWithLocalStatus;
    }
}