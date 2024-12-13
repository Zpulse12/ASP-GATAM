using AutoMapper;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetAllAuth0UsersQuery : IRequest<List<UserDTO>> { }

    public class GetAllAuth0UsersQueryHandler : IRequestHandler<GetAllAuth0UsersQuery, List<UserDTO>>
    {
        private readonly IManagementApi _auth0Repository;
        private readonly IMapper _mapper;

        public GetAllAuth0UsersQueryHandler(IManagementApi auth0Repository, IMapper mapper)
        {
            _auth0Repository = auth0Repository;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(GetAllAuth0UsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _auth0Repository.GetAllUsersAsync();
            var userDTOs = _mapper.Map<List<UserDTO>>(users);

            foreach (var userDTO in userDTOs)
            {
                var roles = await _auth0Repository.GetRolesByUserId(userDTO.Id);
                //userDTO.RolesIds = roles.ToList();
            }

            return userDTOs;
        }
    }
}
