using AutoMapper;
using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;

namespace Gatam.Application.CQRS.Questions
{
    public class GetVisibleQuestionsForFollowerQuery : IRequest<List<Question>>
    {
        public required string FollowerId { get; set; }
    }

    public class GetVisibleQuestionsForFollowerQueryHandler : IRequestHandler<GetVisibleQuestionsForFollowerQuery, List<Question>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVisibleQuestionsForFollowerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Question>> Handle(GetVisibleQuestionsForFollowerQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.QuestionRepository.GetVisibleQuestionsForFollower(request.FollowerId);
        }
    }
} 