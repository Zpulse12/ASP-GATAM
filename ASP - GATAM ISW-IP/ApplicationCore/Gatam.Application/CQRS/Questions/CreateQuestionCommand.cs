using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Domain;
using Gatam.Application.Interfaces;
using FluentValidation;

namespace Gatam.Application.CQRS.Questions
{
    public class CreateQuestionCommand : IRequest<Question>
    {
        public required Question question { get; set; }
    }
    public class CreateQuestionCommandValidator : AbstractValidator<Question>
    {
        private readonly short _minimumQuestionLength = 15;
        private readonly short _maximumQuestionLength = 512;
        public CreateQuestionCommandValidator() 
        {
            RuleFor(question => question.QuestionTitle).NotEmpty().WithMessage("Je moet een vraag meegeven");
            RuleFor(question => question.QuestionAnswer).NotEmpty().WithMessage("je moet antwoorden meegeven");
            RuleFor(question => question.QuestionTitle).NotNull().WithMessage("Je moet een vraag meegeven");
            RuleFor(question => question.QuestionTitle).NotNull().WithMessage("Je moet een vraag meegeven");
            RuleFor(question => question.QuestionTitle).MinimumLength(_minimumQuestionLength).WithMessage($"Je vraag is te kort. Je vraag moet minstens {_minimumQuestionLength} tekens bevatten");
            RuleFor(question => question.QuestionTitle).MinimumLength(_maximumQuestionLength).WithMessage($"Je vraag is te lang. Je vraag mag maximaal {_maximumQuestionLength} tekens bevatten");
        }
    }
    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Question>
    {
        private readonly IUnitOfWork? _uow;
        public CreateQuestionCommandHandler(IUnitOfWork? uow) { _uow = uow; }
        public async Task<Question> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            await _uow.QuestionRepository.Create(request.question);
            return request.question;
        }
    }
}
