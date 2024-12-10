using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module
{
    public class CreateModuleCommand : IRequest<ApplicationModule>
    {
        public ModuleDTO _module { get; set; }
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
        private readonly IMapper _mapper;

        public CreateModuleCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }
        public async Task<ApplicationModule> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            await _uow.ModuleRepository.Create(_mapper.Map<ApplicationModule>(request._module));
            foreach (var question in request._module.Questions)
            {
                var mappedQuestion = _mapper.Map<Question>(question);
                mappedQuestion.ApplicationModuleId = request._module.Id;
                await _uow.QuestionRepository.Create(mappedQuestion);
            }
            await _uow.Commit();

            var createdModule = await _uow.ModuleRepository.FindByIdWithQuestions(request._module.Id);
            return createdModule;
        }
    }

}
