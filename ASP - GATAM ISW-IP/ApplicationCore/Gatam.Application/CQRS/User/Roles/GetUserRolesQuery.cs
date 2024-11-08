using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User.Roles
{
    public class GetUserRolesQuery : IRequest<List<string>>
    {
        public string UserId { get; set; }
    }
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<string>>
    {
        private readonly IManagementApi _auth0Repository;

        public GetUserRolesQueryHandler(IManagementApi auth0Repository)
        {
            _auth0Repository = auth0Repository;
        }
        public async Task<List<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<string> _result = await _auth0Repository.GetRolesByUserId(request.UserId);
            return _result.ToList();
        }
    }
}
