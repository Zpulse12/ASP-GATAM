using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;

namespace Gatam.Application.CQRS.Questions
{
    public class CreateSettingCommand : IRequest<QuestionSettingDTO>
    {
        public required string UserModuleId { get; set; }
        public required string QuestionId { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    public class CreateSettingCommandValidator : AbstractValidator<CreateSettingCommand>
    {
        public CreateSettingCommandValidator()
        {
            RuleFor(x => x.UserModuleId)
                .NotEmpty().WithMessage("UserModuleId is verplicht");
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId is verplicht");
        }
    }

    public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, QuestionSettingDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSettingCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<QuestionSettingDTO> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
        {
            var setting = _mapper.Map<UserModuleQuestionSetting>(request);
            await _uow.UserModuleQuestionSettingRepository.Create(setting);
            await _uow.Commit();

            return _mapper.Map<QuestionSettingDTO>(setting);
        }
    }
} 