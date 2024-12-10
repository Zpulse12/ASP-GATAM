using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class GetAllUserModules : IRequest<IEnumerable<UserModuleDTO>>
    {
        public GetAllUserModules() { }
    }

    public class GetAllUserModulesHandler : IRequestHandler<GetAllUserModules, IEnumerable<UserModuleDTO>>
    {

        private readonly IUnitOfWork? _uow;
        private readonly IMapper _mapper;

        public GetAllUserModulesHandler(IUnitOfWork? uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

        public async Task<IEnumerable<UserModuleDTO>> Handle(GetAllUserModules request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<UserModuleDTO>>(await _uow.UserModuleRepository.GetAllAsync(x => x.Module, x => x.User, x=> x.UserGivenAnswers, x => x.UserQuestions));
        }
    }
}
