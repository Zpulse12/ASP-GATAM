using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
    public class UpdateQuestionCommand : IRequest<Question>
    {
        public string Id { get; set; }

        public required Question Question { get; set; }
    }
    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly short _minimumQuestionLength = 15;
        private readonly short _maximumQuestionLength = 512;

        public UpdateQuestionCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(q => q.Question.QuestionTitle).NotEmpty().WithMessage("Je moet een vraag meegeven");
            RuleFor(q => q.Question.QuestionTitle).NotNull().WithMessage("Je moet een vraag meegeven");
            RuleFor(q => q.Question.Answers).NotEmpty().WithMessage("Je moet een antwoord meegeven");
            RuleFor(q => q.Question.QuestionType).NotEmpty().WithMessage("Je moet een type meegeven");
            RuleFor(q => q.Question.ApplicationModuleId).NotEqual(q => q.Question.Id).WithMessage("Module heeft dezelfde Id als de vraag en dit zorgt voor conflicten. Probeer een nieuwe vraag aan te maken.");
            RuleFor(q => q.Question.QuestionTitle).MinimumLength(_minimumQuestionLength).WithMessage($"Je vraag is te kort. Je vraag moet minstens {_minimumQuestionLength} tekens bevatten");
            RuleFor(q => q.Question.QuestionTitle).MaximumLength(_maximumQuestionLength).WithMessage($"Je vraag is te lang. Je vraag mag maximaal {_maximumQuestionLength} tekens bevatten");

        }
    }

    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, Question>
    {
        private readonly IUnitOfWork _uow;
        public UpdateQuestionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Question> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {

            await _uow.QuestionRepository.UpdateQuestionAndAnswers(request.Question);
            
            await _uow.Commit();

            return request.Question;

        }
    }
}
