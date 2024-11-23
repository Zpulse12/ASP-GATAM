using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
     public class UpdateQuestionVisibilityCommand : IRequest<UserQuestion>
    {
        public string UserQuestionId { get; set; }
        public bool IsVisible { get; set; }
    }

    public class UpdateQuestionVisibilityCommandValidator : AbstractValidator<UpdateQuestionVisibilityCommand>
    {
        public UpdateQuestionVisibilityCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserQuestionId)
                .NotEmpty()
                .WithMessage("QuestionSetting ID is required");
            RuleFor(x => x.UserQuestionId)
                .MustAsync(async (UserQuestionId, cancellationToken) =>
                {
                    var setting = await uow.UserQuestionRepository.GetQuestionSettingById(UserQuestionId);
                    return setting != null;

                }).WithMessage("Question setting doesn't exist");
        }
    }
    public class UpdateQuestionVisibilityCommandHandler : IRequestHandler<UpdateQuestionVisibilityCommand, UserQuestion>
    {
        private readonly IUnitOfWork _uow;

        public UpdateQuestionVisibilityCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserQuestion> Handle(UpdateQuestionVisibilityCommand request, CancellationToken cancellationToken)
        {
            var setting = await _uow.UserQuestionRepository.GetQuestionSettingById(request.UserQuestionId);
            setting.IsVisible = request.IsVisible;
            await _uow.UserQuestionRepository.Update(setting);
            return setting;
        }
    }
} 