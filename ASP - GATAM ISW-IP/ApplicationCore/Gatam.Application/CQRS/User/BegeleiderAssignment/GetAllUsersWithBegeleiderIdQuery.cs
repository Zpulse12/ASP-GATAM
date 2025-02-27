﻿using AutoMapper;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class GetAllUsersWithBegeleiderIdQuery : IRequest<IEnumerable<UserDTO>>
    {
        public GetAllUsersWithBegeleiderIdQuery()
        {

        }
    }
    public class GetAllUsersWithBegeleiderIdQueryHandler : IRequestHandler<GetAllUsersWithBegeleiderIdQuery, IEnumerable<UserDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllUsersWithBegeleiderIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersWithBegeleiderIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<UserDTO>>(await _uow.UserRepository.GetAllAsync());
        }
    }

}
