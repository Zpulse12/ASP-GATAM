using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.User;
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
    public class FindUserModuleIdQuery : IRequest<UserModuleDTO>
    {
        public string UserModuleId { get; set; }

    }
    public class FindUserModuleIdQueryHandler : IRequestHandler<FindUserModuleIdQuery, UserModuleDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FindUserModuleIdQueryHandler(IUnitOfWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserModuleDTO> Handle(FindUserModuleIdQuery request, CancellationToken cancellationToken)
        {
            var userModule = await _uow.UserModuleRepository.FindById(request.UserModuleId);
            return _mapper.Map<UserModuleDTO>(userModule);
        }
    }
}
