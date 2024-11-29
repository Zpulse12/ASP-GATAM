using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
    public class GetQuestionByIdQuery: IRequest<Question>
    {
        public string Id { get; set; }
    }

    public class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Question>
    {

        private readonly IUnitOfWork _uow;
        public GetQuestionByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Question> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _uow.QuestionRepository.GetQuestionAndAnswers(request.Id);
        }

    }
}
