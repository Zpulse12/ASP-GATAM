using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.Questions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Module
{
    public class GetModuleByIdQuery : IRequest<ModuleDTO>
    {
        public string Id { get; set; }
    }
    public class GetModuleByIdQueryHandler : IRequestHandler<GetModuleByIdQuery, ModuleDTO>
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public GetModuleByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ModuleDTO> Handle(GetModuleByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<ModuleDTO>(await _uow.ModuleRepository.FindById(request.Id));
        }

    }
}
