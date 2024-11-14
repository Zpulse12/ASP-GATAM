using Gatam.Application.Extensions;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDTO>>
    {
        //public  UserDTO _user { get; set; }
        public GetAllUsersQuery()
        {
        }
    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
    {
        private readonly IManagementApi _auth0Repository;
        private readonly EnvironmentWrapper _environmentWrapper;

        public GetAllUsersQueryHandler(IManagementApi auth0Repository, EnvironmentWrapper environmentWrapper)
        {
            _auth0Repository = auth0Repository;
            _environmentWrapper = environmentWrapper;
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            //request._user.Name = AESProvider.Decrypt(request._user.Name, _environmentWrapper.KEY);
            //request._user.Surname = AESProvider.Decrypt(request._user.Surname, _environmentWrapper.KEY);
            //request._user.Username = AESProvider.Decrypt(request._user.Username, _environmentWrapper.KEY);
            //request._user.Email = AESProvider.Decrypt(request._user.Email, _environmentWrapper.KEY);
            //request._user.PhoneNumber = AESProvider.Decrypt(request._user.PhoneNumber, _environmentWrapper.KEY);

            var users = await _auth0Repository.GetAllUsersAsync();
            return users;
        }

    }
}
