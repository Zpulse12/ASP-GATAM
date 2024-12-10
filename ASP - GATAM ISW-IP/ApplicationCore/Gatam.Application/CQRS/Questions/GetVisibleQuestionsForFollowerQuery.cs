using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using AutoMapper;

namespace Gatam.Application.CQRS.Questions
{
    public class GetVisibleQuestionsForFollowerQuery : IRequest<List<QuestionDTO>>
    {
        public required string FollowerId { get; set; }
    }

    public class GetVisibleQuestionsForFollowerQueryHandler : IRequestHandler<GetVisibleQuestionsForFollowerQuery, List<QuestionDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVisibleQuestionsForFollowerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<QuestionDTO>> Handle(GetVisibleQuestionsForFollowerQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<List<QuestionDTO>>(await _unitOfWork.QuestionRepository.GetVisibleQuestionsForFollower(request.FollowerId));
        }
    }
} 