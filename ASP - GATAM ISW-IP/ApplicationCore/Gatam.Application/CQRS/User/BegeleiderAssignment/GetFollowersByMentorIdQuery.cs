using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class GetFollowersByMentorIdQuery : IRequest<List<ApplicationUser>>
    {
        public string MentorId { get; set; }
        
    }

    public class GetFollowersByMentorIdQueryHandler : IRequestHandler<GetFollowersByMentorIdQuery, List<ApplicationUser>>
    {
        private readonly IUnitOfWork _uow;

        public GetFollowersByMentorIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<ApplicationUser>> Handle(GetFollowersByMentorIdQuery request, CancellationToken cancellationToken)
        {
            return await _uow.UserRepository.GetUsersForBegeleiderAsync(request.MentorId);
        }
    }
}