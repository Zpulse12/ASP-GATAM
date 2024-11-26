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
    public class UpdateModuleCommand : IRequest<ModuleDTO>
    {
        public string Id { get; set; }

        public required ModuleDTO Module { get; set; }
    }
    public class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, ModuleDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UpdateModuleCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;

        }

        public async Task<ModuleDTO> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
        {

            await _uow.ModuleRepository.Update(_mapper.Map<ApplicationModule>(request.Module));

            await _uow.Commit();

            return request.Module;

        }
    }
}
