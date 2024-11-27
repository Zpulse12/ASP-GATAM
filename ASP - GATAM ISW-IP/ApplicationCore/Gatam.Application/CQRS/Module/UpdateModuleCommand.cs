using AutoMapper;
using FluentValidation;
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
    public class UpdateModuleCommandValidators : AbstractValidator<UpdateModuleCommand>
    {
        private readonly IUnitOfWork _uow;
        public UpdateModuleCommandValidators(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Module.Category).NotEmpty().WithMessage("Category mag niet leeg zijn");
            RuleFor(x => x.Module.Category).NotNull().WithMessage("Category mag niet null zijn");
            RuleFor(x => x.Module.Title).NotEmpty().WithMessage("Titel mag niet leeg zijn");
            RuleFor(x => x.Module.Title).NotNull().WithMessage("Titel mag niet null zijn");
            RuleFor(x => x.Module.Title).MustAsync(BeUniqueTitle).WithMessage("Titel bestaat al."); ;

        }
        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var existingModule = await _uow.ModuleRepository.FindByProperty("Title", title);
            return existingModule == null; 
        }
    }
        public class UpdateModuleCommandHandler : IRequestHandler<UpdateModuleCommand, ModuleDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateModuleCommandHandler(IUnitOfWork uow, IMapper mapper, IUserRepository userRepository)
        {
            _uow = uow;
            _mapper = mapper;
            _userRepository=userRepository;
        }

        public async Task<ModuleDTO> Handle(UpdateModuleCommand request, CancellationToken cancellationToken)
        {
            var usersWithModule = await _userRepository.GetUsersByModuleIdAsync(request.Id);

            if (usersWithModule.SelectMany(u => u.UserModules)
                               .Any(um => um.ModuleId == request.Id && um.State == UserModuleState.InProgress))
            {
                throw new InvalidOperationException("De module kan niet worden verwijderd omdat deze in gebruik is door een trajectvolger.");
            }

            var moduleEntity = _mapper.Map<ApplicationModule>(request.Module);

            await _uow.ModuleRepository.UpdateModuleWithQuestions(moduleEntity);

            await _uow.Commit();

            return request.Module;

        }
    }
}
