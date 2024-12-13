using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
    namespace Gatam.Application.CQRS.Questions
    {
        public class UpdateQuestionPriorityCommand : IRequest<UserQuestion>
        {
            public string UserQuestionId { get; set; }
            public QuestionPriority Priority { get; set; }
        }

        public class UpdateQuestionPriorityCommandValidator : AbstractValidator<UpdateQuestionPriorityCommand>
        {
            public UpdateQuestionPriorityCommandValidator(IUnitOfWork uow)
            {
                RuleFor(x => x.UserQuestionId)
                    .NotEmpty()
                    .WithMessage("QuestionSetting ID mag niet leeg zijn.");
                RuleFor(x => x.UserQuestionId)
                    .MustAsync(async (UserQuestionId, cancellationToken) =>
                    {
                        var setting = await uow.UserQuestionRepository.GetQuestionSettingById(UserQuestionId);
                        return setting != null;

                    }).WithMessage("Question setting bestaat niet");
                RuleFor(x => x.Priority).Must((Priority) =>
                {
                    return Enum.IsDefined(typeof(QuestionPriority), Priority);
                }).WithMessage("Foute prioriteit");
            }
        }
        public class UpdateQuestionPriorityCommandHandler : IRequestHandler<UpdateQuestionPriorityCommand, UserQuestion>
        {
            private readonly IUnitOfWork _uow;

            public UpdateQuestionPriorityCommandHandler(IUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<UserQuestion> Handle(UpdateQuestionPriorityCommand request, CancellationToken cancellationToken)
            {
                var setting = await _uow.UserQuestionRepository.GetQuestionSettingById(request.UserQuestionId);
                setting.QuestionPriority = request.Priority;
                await _uow.UserQuestionRepository.Update(setting);
                await _uow.Commit();
                return setting;
            }
        }
    }
}
