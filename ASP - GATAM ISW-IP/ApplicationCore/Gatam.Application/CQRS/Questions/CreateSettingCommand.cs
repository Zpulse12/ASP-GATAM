using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using AutoMapper;
using FluentValidation;

namespace Gatam.Application.CQRS.Questions
{
    public class CreateSettingCommand : IRequest<UserModuleQuestionSettingDTO>
    {
        public required string UserModuleId { get; set; }
        public required string QuestionId { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsEditable { get; set; } = false;
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

    public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, UserModuleQuestionSettingDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSettingCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserModuleQuestionSettingDTO> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
        {
            var setting = _mapper.Map<UserModuleQuestionSetting>(request);
            await _uow.UserModuleQuestionSettingRepository.Create(setting);
            await _uow.Commit();

            return _mapper.Map<UserModuleQuestionSettingDTO>(setting);
        }
    }
} 