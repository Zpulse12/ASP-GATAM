using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module
{
    public class GetAllModulesQuery : IRequest<IEnumerable<ModuleDTO>>
    {
        public GetAllModulesQuery()
        {
            
        }
    }
    public class GetAllModulesQueryHandler : IRequestHandler<GetAllModulesQuery, IEnumerable<ModuleDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllModulesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleDTO>> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ModuleDTO>>(await _uow.ModuleRepository.GetAllAsync(x => x.Questions 
                ));
        }
    }
}
