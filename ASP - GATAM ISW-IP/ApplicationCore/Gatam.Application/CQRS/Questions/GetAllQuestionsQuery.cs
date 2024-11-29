﻿using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
    public class GetAllQuestionsQuery : IRequest<IEnumerable<Question>>
    {
        public GetAllQuestionsQuery() { }
    }

    public class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<Question>>
    {

        private readonly IUnitOfWork? _uow;
        public GetAllQuestionsQueryHandler(IUnitOfWork? uow) { _uow = uow; }

        public async Task<IEnumerable<Question>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            return await _uow.QuestionRepository.GetAllAsync(x => x.Answers);
        }
    }
}
