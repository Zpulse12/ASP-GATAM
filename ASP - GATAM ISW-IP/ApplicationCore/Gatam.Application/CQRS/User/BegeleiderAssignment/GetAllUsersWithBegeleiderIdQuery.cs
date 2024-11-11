using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class GetAllUsersWithBegeleiderIdQuery : IRequest<IEnumerable<ApplicationUser>>
    {
        public GetAllUsersWithBegeleiderIdQuery()
        {

        }
    }
    public class GetAllUsersWithBegeleiderIdQueryHandler : IRequestHandler<GetAllUsersWithBegeleiderIdQuery, IEnumerable<ApplicationUser>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllUsersWithBegeleiderIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationUser>> Handle(GetAllUsersWithBegeleiderIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ApplicationUser>>(await _uow.UserRepository.GetAllAsync());
        }
    }

}
