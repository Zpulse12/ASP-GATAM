using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class FindUserModuleIdQuery : IRequest<UserModuleDTO>
    {
        public string UserModuleId { get; set; }

    }
    public class FindUserModuleIdQueryValidator : AbstractValidator<FindUserModuleIdQuery>
    {
        private readonly IUnitOfWork _uow;

        public FindUserModuleIdQueryValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.UserModuleId)
                .NotEmpty().WithMessage("UserModuleId mag niet leeg zijn.")
                .MustAsync(async (userModuleId, cancellationToken) => await UserModuleExists(userModuleId))
                .WithMessage("UserModuleId bestaat niet.");
        }
        private async Task<bool> UserModuleExists(string userModuleId)
        {
            var userModule = await _uow.UserModuleRepository.FindById(userModuleId);
            return userModule != null;
        }
    }
    public class FindUserModuleIdQueryHandler : IRequestHandler<FindUserModuleIdQuery, UserModuleDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public FindUserModuleIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserModuleDTO> Handle(FindUserModuleIdQuery request, CancellationToken cancellationToken)
        {
            var userModule = await _uow.UserModuleRepository.FindByIdModuleWithIncludes(request.UserModuleId);
            userModule.UserQuestions = userModule.UserQuestions
           .OrderBy(q => q.QuestionPriority)
           .ToList(); ;
            return _mapper.Map<UserModuleDTO>(userModule);
        }
    }
}
