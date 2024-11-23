using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Module.UserModules;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Questions
{
    public class GetAllQuestionByModuleIdQuery : IRequest<List<Question>>
    {
        public string ModuleId { get; set; }
    }
    public class GetAllQuestionByModuleIdQueryHandler : IRequestHandler<GetAllQuestionByModuleIdQuery, List<Question>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetAllQuestionByModuleIdQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<List<Question>> Handle(GetAllQuestionByModuleIdQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetQuestionsByModuleIdAsync(request.ModuleId, true);
            return questions;
        }
    }
}
