using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.CQRS.Questions;
using FluentValidation;
using Gatam.Application.Interfaces;

namespace ApplicationCore.CQRS.Questions
{
     public class UpdateQuestionVisibilityCommand : IRequest<bool>
    {
        public string UserModuleQuestionSettingId { get; set; }
        public bool IsVisible { get; set; }
    }

    public class UpdateQuestionVisibilityCommandValidator : AbstractValidator<UpdateQuestionVisibilityCommand>
    {
        public UpdateQuestionVisibilityCommandValidator()
        {
            RuleFor(x => x.UserModuleQuestionSettingId)
                .NotEmpty()
                .WithMessage("QuestionSetting ID is required");
        }
    }
    public class UpdateQuestionVisibilityCommandHandler : IRequestHandler<UpdateQuestionVisibilityCommand, bool>
    {
        private readonly IQuestionRepository _questionRepository;

        public UpdateQuestionVisibilityCommandHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<bool> Handle(UpdateQuestionVisibilityCommand request, CancellationToken cancellationToken)
        {
            var setting = await _questionRepository.GetQuestionSettingById(request.UserModuleQuestionSettingId);
            if (setting == null) return false;

            setting.IsVisible = request.IsVisible;
            await _questionRepository.UpdateQuestionSetting(setting);
            return true;
        }
    }
} 