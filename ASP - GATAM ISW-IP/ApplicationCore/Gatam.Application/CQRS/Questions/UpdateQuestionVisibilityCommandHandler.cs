using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Questions
{
     public class UpdateQuestionVisibilityCommand : IRequest<UserModuleQuestionSetting>
    {
        public string UserModuleQuestionSettingId { get; set; }
        public bool IsVisible { get; set; }
    }

    public class UpdateQuestionVisibilityCommandValidator : AbstractValidator<UpdateQuestionVisibilityCommand>
    {
        public UpdateQuestionVisibilityCommandValidator(IUnitOfWork uow)
        {
            RuleFor(x => x.UserModuleQuestionSettingId)
                .NotEmpty()
                .WithMessage("QuestionSetting ID is required");
            RuleFor(x => x.UserModuleQuestionSettingId)
                .MustAsync(async (userModuleQuestionSettingId, cancellationToken) =>
                {
                    var setting = await uow.UserModuleQuestionSettingRepository.GetQuestionSettingById(userModuleQuestionSettingId);
                    return setting != null;

                }).WithMessage("Question setting doesn't exist");
        }
    }
    public class UpdateQuestionVisibilityCommandHandler : IRequestHandler<UpdateQuestionVisibilityCommand, UserModuleQuestionSetting>
    {
        private readonly IUnitOfWork _uow;

        public UpdateQuestionVisibilityCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserModuleQuestionSetting> Handle(UpdateQuestionVisibilityCommand request, CancellationToken cancellationToken)
        {
            var setting = await _uow.UserModuleQuestionSettingRepository.GetQuestionSettingById(request.UserModuleQuestionSettingId);
            setting.IsVisible = request.IsVisible;
            await _uow.UserModuleQuestionSettingRepository.UpdateQuestionSetting(setting);
            return setting;
        }
    }
} 