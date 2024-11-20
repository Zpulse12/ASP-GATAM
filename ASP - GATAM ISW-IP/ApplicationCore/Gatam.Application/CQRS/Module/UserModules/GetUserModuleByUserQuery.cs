using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class GetUserModuleByUserQuery : IRequest<List<UserModuleDTO>>
    {
        public string UserId { get; set; }

        public GetUserModuleByUserQuery(string userId)
        {
            UserId = userId;
        }
    }
    public class GetUserModuleByUserQueryHandler : IRequestHandler<GetUserModuleByUserQuery, List<UserModuleDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserModuleByUserQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserModuleDTO>> Handle(GetUserModuleByUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserWithModules(request.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return (List<UserModuleDTO>)user.UserModules;
        }
    }
}
