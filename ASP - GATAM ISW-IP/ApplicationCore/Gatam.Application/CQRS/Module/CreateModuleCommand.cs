using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module
{
    public class CreateModuleCommand : IRequest<ModuleDTO>
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
            RuleFor(x => x._module.Title).MustAsync(BeUniqueTitle).WithMessage("Titel bestaat al.");

            RuleFor(q => q._module.Questions).NotEmpty().WithMessage("Je moet een vraag meegeven");
            RuleForEach(q => q._module.Questions)
             .Must(q => q.QuestionAnswers != null && q.QuestionAnswers.Any())
             .WithMessage("Elke vraag moet minstens één antwoord hebben.");

        }
        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            var existingModule = await _uow.ModuleRepository.FindByProperty("Title", title);
            return existingModule == null; // true als de title uniek is
        }
    }
    public class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, ModuleDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateModuleCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }
        public async Task<ModuleDTO> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
        {
            await _uow.ModuleRepository.Create(_mapper.Map<ApplicationModule>(request._module));
            foreach (var question in request._module.Questions)
            {
                var existingQuestion = await _uow.QuestionRepository.FindFirstAsync(x => x.QuestionTitle == question.QuestionTitle && x.QuestionType == question.QuestionType);
                if (existingQuestion!= null)
                {
                    existingQuestion.ApplicationModuleId = request._module.Id;
                    await _uow.QuestionRepository.Update(_mapper.Map<Question>(existingQuestion));
                }
                else
                {
                    question.QuestionTitle = HandleTitleUniformEntry(question.QuestionTitle);
                    question.ApplicationModuleId = request._module.Id;
                    await _uow.QuestionRepository.Create(_mapper.Map<Question>(question));
                }
            }
            await _uow.Commit();

            var createdModule = await _uow.ModuleRepository.FindByIdWithQuestions(request._module.Id);
            return _mapper.Map<ModuleDTO>(createdModule);

        }
        private string HandleTitleUniformEntry(string s)
        {
            if (!s.EndsWith("?"))
            {
                s = s + "?";
            }
            if (!Char.IsUpper(s[0]))
            {
                s = StringHelper.CapitalizeFirstLetter(s);
            }
            return s;
        }
    }

}
