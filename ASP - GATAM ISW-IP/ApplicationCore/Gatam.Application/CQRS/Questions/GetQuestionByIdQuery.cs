
using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using Microsoft.AspNetCore.Http.Features;

namespace Gatam.Application.CQRS.Questions
{
    public class GetQuestionByIdQuery: IRequest<Question>
    {
        public string Id { get; set; }
    }

    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Question>
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetQuestionByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Question> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _uow.QuestionRepository.GetQuestionAndAnswers(request.Id);
        }

    }
}
