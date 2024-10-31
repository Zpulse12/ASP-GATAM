using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.InvitationTeam;
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
    public class CreateModuleCommand : IRequest<ApplicationModule>
    {
        public required ApplicationModule _module { get; set; }
    }
    public class CreateModuleCommandValidators : AbstractValidator<CreateModuleCommand>
    {
        private readonly IUnitOfWork _uow;
        public CreateModuleCommandValidators(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x._module.Category)
                 .NotEmpty()
                 .WithMessage("Category mag niet leeg zijn");
            RuleFor(x => x._module.Category)
                .NotNull()
                .WithMessage("Category mag niet null zijn");
            RuleFor(x => x._module.Title)
                .NotEmpty()
                .WithMessage("Titel mag niet leeg zijn");
            RuleFor(x => x._module.Title)
                .NotNull()
                .WithMessage("Titel mag niet null zijn");
        }
    }

    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, ApplicationModule>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public CreateModuleCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<ApplicationModule> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            await uow.ModuleRepository.Create(request._module);
            await uow.commit();
            return request._module;
        }
    }

}
