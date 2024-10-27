using AutoMapper;
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
    public class GetAllModulesQuery : IRequest<IEnumerable<ApplicationModule>>
    {
        public GetAllModulesQuery()
        {
            
        }
    }
    public class GetAllModulesQueryHandler : IRequestHandler<GetAllModulesQuery, IEnumerable<ApplicationModule>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllModulesQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ApplicationModule>> Handle(GetAllModulesQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<IEnumerable<ApplicationModule>>(await _uow.ModuleRepository.GetAllAsync(
               q => q.Questions
                ));
        }
    }
}
