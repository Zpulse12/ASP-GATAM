﻿using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetAllInvitationsQuery : IRequest<IEnumerable<TeamInvitation>>
    {
        public GetAllInvitationsQuery()
        {
        }

    }
    public class GetAllInvitationsQueryHandler : IRequestHandler<GetAllInvitationsQuery, IEnumerable<TeamInvitation>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllInvitationsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamInvitation>> Handle(GetAllInvitationsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<TeamInvitation>>(await _uow.TeamInvitationRepository.GetAllAsync(
                t => t.applicationUser
            ));
        }

    }
}
