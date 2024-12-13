using AutoMapper;
using Gatam.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.User.Roles
{
    public class GetUsersByRoleQuery : IRequest<List<UserDTO>>
    {
        public string RoleId { get; set; }
    }
    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersByRoleQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllAsync(x => x.UserRoles);

            var usersWithRole = allUsers
                .Where(user => user.UserRoles.Any(role => role.RoleId == request.RoleId))
                .ToList();

            return _mapper.Map<List<UserDTO>>(usersWithRole);
        }
    }


}
