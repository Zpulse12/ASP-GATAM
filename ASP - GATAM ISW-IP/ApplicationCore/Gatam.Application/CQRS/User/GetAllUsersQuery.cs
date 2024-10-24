using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDTO>>
    {
        public GetAllUsersQuery()
        {
        }
    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
    {
        private readonly IManagementApi _auth0Repository;


        public GetAllUsersQueryHandler(IManagementApi auth0Repository)
        {
            _auth0Repository = auth0Repository;
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _auth0Repository.GetAllUsersAsync();
            return users;
        }

    }
}
