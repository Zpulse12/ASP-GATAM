using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class GetFollowersByMentorIdQuery : IRequest<List<ApplicationUser>>
    {
        public string BegeleiderId { get; }

        public GetFollowersByMentorIdQuery(string begeleiderId)
        {
            BegeleiderId = begeleiderId;
        }
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
            return await _uow.UserRepository.GetUsersForBegeleiderAsync(request.BegeleiderId);
        }
    }
}