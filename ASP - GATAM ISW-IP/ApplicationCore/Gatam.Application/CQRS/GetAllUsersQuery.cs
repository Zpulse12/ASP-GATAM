using AutoMapper;
using Gatam.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Gatam.Application.CQRS
{
    public class GetAllUsersQuery : IRequest<IEnumerable<TeamInvitationDTO>>
    {
        public GetAllUsersQuery()
        {
        }

    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<TeamInvitationDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamInvitationDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<TeamInvitationDTO>>(await _uow.UserRepository.GetAllAsync());
        }

    }
}
