using AutoMapper;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
    public class GetAllQuestionsQuery : IRequest<IEnumerable<QuestionDTO>>
    {
        public GetAllQuestionsQuery() { }
    }

    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<QuestionDTO>>
    {

        private readonly IUnitOfWork? _uow;
        private readonly IMapper _mapper;

        public GetAllQuestionsQueryHandler(IUnitOfWork? uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

        public async Task<IEnumerable<QuestionDTO>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<QuestionDTO>>(await _uow.QuestionRepository.GetAllAsync(x => x.Answers));
        }
    }
}
