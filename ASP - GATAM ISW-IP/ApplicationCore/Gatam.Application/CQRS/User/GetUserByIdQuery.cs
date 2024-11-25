using AutoMapper;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetUserByIdQuery: IRequest<UserDTO>
    {
        public string UserId { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;


        public GetUserByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserDTO>(await _uow.UserRepository.FindById(request.UserId));
        }

    }
}
