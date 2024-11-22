using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;

namespace Gatam.Application.CQRS.Questions
{
    public class CreateSettingCommand : IRequest<bool>
    {
        public string QuestionId { get; set; }
        public string UserModuleId { get; set; }
    }

    public class CreateSettingCommandValidator : AbstractValidator<CreateSettingCommand>
    {
        public CreateSettingCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("QuestionId is verplicht");
        }
    }

    public class CreateSettingCommandHandler : IRequestHandler<CreateSettingCommand, bool>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CreateSettingCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
        {
            var setting = _mapper.Map<UserModuleQuestionSetting>(request);
            await _uow.UserModuleQuestionSettingRepository.Create(setting);
            await _uow.Commit();

            return true;
        }
    }
} 