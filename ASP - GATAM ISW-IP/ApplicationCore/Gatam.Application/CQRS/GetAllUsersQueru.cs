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
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDTO>>
    {
        public GetAllUsersQuery()
        {
        }

    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<UserDTO>>(await _uow.UserRepository.GetAllUsers());
        }

    }
}
