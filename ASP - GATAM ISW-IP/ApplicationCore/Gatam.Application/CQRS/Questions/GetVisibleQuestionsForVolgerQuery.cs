using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gatam.Application.Interfaces;
using Gatam.Domain;

namespace Gatam.Application.CQRS.Questions
{
    public class GetVisibleQuestionsForVolgerQuery : IRequest<List<Question>>
    {
        public required string VolgerId { get; set; }
    }

    public class GetVisibleQuestionsForVolgerQueryHandler : IRequestHandler<GetVisibleQuestionsForVolgerQuery, List<Question>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVisibleQuestionsForVolgerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Question>> Handle(GetVisibleQuestionsForVolgerQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.QuestionRepository.GetVisibleQuestionsForVolger(request.VolgerId);
        }
    }
} 