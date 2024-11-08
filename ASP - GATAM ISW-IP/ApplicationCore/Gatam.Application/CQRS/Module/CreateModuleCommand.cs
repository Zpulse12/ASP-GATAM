using AutoMapper;
using FluentValidation;
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
        public ApplicationModule _module { get; set; }
    }
    public class CreateModuleCommandValidators : AbstractValidator<CreateModuleCommand>
    {
        private readonly IUnitOfWork _uow;
        public CreateModuleCommandValidators(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x._module.Category).NotEmpty().WithMessage("Category mag niet leeg zijn");
            RuleFor(x => x._module.Category).NotNull().WithMessage("Category mag niet null zijn");
            RuleFor(x => x._module.Title).NotEmpty().WithMessage("Titel mag niet leeg zijn");
            RuleFor(x => x._module.Title).NotNull().WithMessage("Titel mag niet null zijn");
            RuleFor(x => x._module.Title).MustAsync(BeUniqueTitle).WithMessage("Titel bestaat al."); ;

        }
        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var existingModule = await _uow.ModuleRepository.FindByProperty("Title", title);
            return existingModule == null; // true als de title uniek is
        }
    }


    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, ApplicationModule>
    {
        private readonly IUnitOfWork _uow;
        public CreateModuleCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
        }
        public async Task<ApplicationModule> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            await _uow.ModuleRepository.Create(request._module);
            await _uow.Commit();
            return request._module;
        }
    }

}
