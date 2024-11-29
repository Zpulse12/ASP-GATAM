using MediatR;
using Gatam.Domain;
using Gatam.Application.Interfaces;
using FluentValidation;
using Gatam.Application.Extensions;

namespace Gatam.Application.CQRS.Questions
{
    public class CreateQuestionCommand : IRequest<Question>
    {
        public Question question { get; set; }
    }
    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        private readonly short _minimumQuestionLength = 15;
        private readonly short _maximumQuestionLength = 512;
        private readonly IUnitOfWork _uow;

        public CreateQuestionCommandValidator(IUnitOfWork uow) 
        {
            _uow = uow;

            RuleFor(q => q.question.QuestionTitle).NotEmpty().WithMessage("Je moet een vraag meegeven");
            RuleFor(q => q.question.QuestionTitle).NotNull().WithMessage("Je moet een vraag meegeven");
            RuleFor(q => q.question.Answers).NotEmpty().WithMessage("Je moet een antwoord meegeven");
            RuleFor(q => q.question.QuestionType).Must(value => Enum.IsDefined(typeof(QuestionType), (QuestionType)value)).WithMessage("Je moet een type meegeven");
            RuleFor(q => q.question.ApplicationModuleId).NotEqual(q => q.question.Id)
                .WithMessage("Module heeft dezelfde Id als de vraag en dit zorgt voor conflicten. Probeer een nieuwe vraag aan te maken.");
            RuleFor(q => q.question.QuestionTitle)
                .MinimumLength(_minimumQuestionLength)
                .WithMessage($"Je vraag is te kort. Je vraag moet minstens {_minimumQuestionLength} tekens bevatten");
            RuleFor(q => q.question.QuestionTitle)
                .MaximumLength(_maximumQuestionLength)
                .WithMessage($"Je vraag is te lang. Je vraag mag maximaal {_maximumQuestionLength} tekens bevatten");
            RuleFor(q => q)
                .MustAsync(async (command, cancellation) => {
                    var existingQuestion = await _uow.QuestionRepository.GetAllAsync();
                    bool isDuplicate = existingQuestion.Any(q => 
                        q.QuestionTitle.Equals(command.question.QuestionTitle, StringComparison.OrdinalIgnoreCase) && 
                        q.QuestionType == command.question.QuestionType);
                    
                    return !isDuplicate;
                })
                .WithMessage("Deze vraag bestaat al voor dit type vraag. Kies een ander type of pas de vraag aan.");
        }
    }
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Question>
    {
        private readonly IUnitOfWork? _uow;
        public CreateQuestionCommandHandler(IUnitOfWork? uow) { _uow = uow; }
        public async Task<Question> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {

            request.question.QuestionTitle = HandleTitleUniformEntry(request.question.QuestionTitle);
            await _uow.QuestionRepository.Create(request.question);
            await _uow.Commit();
            return request.question;
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
