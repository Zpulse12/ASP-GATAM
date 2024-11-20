using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class GetAllUserModulesQuery : IRequest<IEnumerable<UserModule>>
    {
        public GetAllUserModulesQuery()
        {
        }
    }
    public class GetAllUserModulesQueryHandler : IRequestHandler<GetAllUserModulesQuery, IEnumerable<UserModule>>
    {
        private readonly IUnitOfWork _uow;

        public GetAllUserModulesQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<UserModule>> Handle(GetAllUserModulesQuery request, CancellationToken cancellationToken)
        {
            return await _uow.UserModuleRepository.GetAllAsync();
        }

    }
}
