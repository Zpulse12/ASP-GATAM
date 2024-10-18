using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class GetAllTeamsQuery : IRequest<IEnumerable<ApplicationTeam>>
    {
        public GetAllTeamsQuery()
        {
        }

    }
    public class GetAllTeamsQueryHandler : IRequestHandler<GetAllTeamsQuery, IEnumerable<ApplicationTeam>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllTeamsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationTeam>> Handle(GetAllTeamsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ApplicationTeam>>(await _uow.TeamRepository.GetAllAsync(
                t => t.TeamCreator,
                t => t.TeamInvitations,
                t => ((TeamInvitation)t.TeamInvitations).applicationUser));
        }

        




    }
}
