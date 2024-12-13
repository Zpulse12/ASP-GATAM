using AutoMapper;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
    public class GetQuestionByIdQuery: IRequest<QuestionDTO>
    {
        public string Id { get; set; }
    }

    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDTO>
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetQuestionByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<QuestionDTO> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<QuestionDTO>(await _uow.QuestionRepository.GetQuestionAndAnswers(request.Id));
        }

    }
}
