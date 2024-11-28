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
    public class GetUserModuleByUserQuery : IRequest<List<UserModuleDTO>>
    {
        public string UserId { get; set; }
    }
    public class GetUserModuleByUserQueryValidator : AbstractValidator<GetUserModuleByUserQuery>
    {
        public GetUserModuleByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId mag niet leeg zijn.");
        }
    }
    public class GetUserModuleByUserQueryHandler : IRequestHandler<GetUserModuleByUserQuery, List<UserModuleDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserModuleByUserQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<UserModuleDTO>> Handle(GetUserModuleByUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetUserWithModules(request.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return _mapper.Map<List<UserModuleDTO>>(user.UserModules);
        }
    }
}
