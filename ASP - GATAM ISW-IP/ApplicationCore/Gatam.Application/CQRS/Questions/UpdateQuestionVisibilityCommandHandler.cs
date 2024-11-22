using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
     public class UpdateQuestionVisibilityCommand : IRequest<UserModuleQuestionSetting>
    {
        public string UserModuleQuestionSettingId { get; }
        public bool IsVisible { get; }
    }

    public class UpdateQuestionVisibilityCommandValidator : AbstractValidator<UpdateQuestionVisibilityCommand>
    {
        public UpdateQuestionVisibilityCommandValidator(IQuestionRepository questionRepository)
        {
            RuleFor(x => x.UserModuleQuestionSettingId)
                .NotEmpty()
                .WithMessage("QuestionSetting ID is required");
            RuleFor(x => x.UserModuleQuestionSettingId)
                .MustAsync(async (userModuleQuestionSettingId, cancellationToken) =>
                {
                    var setting = await questionRepository.GetQuestionSettingById(userModuleQuestionSettingId);
                    return setting != null;

                }).WithMessage("Question setting doesn't exist");
        }
    }
    public class UpdateQuestionVisibilityCommandHandler : IRequestHandler<UpdateQuestionVisibilityCommand, UserModuleQuestionSetting>
    {
        private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionVisibilityCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<UserModuleQuestionSetting> Handle(UpdateQuestionVisibilityCommand request, CancellationToken cancellationToken)
        {
            var setting = await _questionRepository.GetQuestionSettingById(request.UserModuleQuestionSettingId);
            setting.IsVisible = request.IsVisible;
            await _questionRepository.UpdateQuestionSetting(setting);
            return setting;
        }
    }
} 