using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class GetFollowersByMentorIdQuery : IRequest<List<UserDTO>>
    {
        public string MentorId { get; set; }
        
    }

    public class GetFollowersByMentorIdQueryHandler : IRequestHandler<GetFollowersByMentorIdQuery, List<UserDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;


        public GetFollowersByMentorIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<UserDTO>> Handle(GetFollowersByMentorIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<UserDTO>>(await _uow.UserRepository.GetUsersForBegeleiderAsync(request.MentorId));
        }
    }
}