using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class UpdateUserModuleStatusCommand : IRequest<UserModule>
    {
        public string UserModuleId { get; set; }
        public UserModuleState State { get; set; }
    }
    public class UpdateUserModuleStatusCommandValidator: AbstractValidator<UpdateUserModuleStatusCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateUserModuleStatusCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.UserModuleId)
                .NotEmpty().WithMessage("UserModuleId mag niet leeg zijn.")
                .MustAsync(async (userModuleId, cancellationToken) => await UserModuleExists(userModuleId))
                .WithMessage("UserModuleId bestaat niet.");
            RuleFor(x => x.UserModuleId).NotEmpty().WithMessage("UserModuleId mag niet leeg zijn");
            RuleFor(x => x.State).NotNull().WithMessage("State mag niet null zijn");
        }
        private async Task<bool> UserModuleExists(string userModuleId)
        {
            var userModule = await _uow.UserModuleRepository.FindById(userModuleId);
            return userModule != null;
        }
    }
    public class UpdateUserModuleStatusHandler : IRequestHandler<UpdateUserModuleStatusCommand, UserModule>
    {
        private readonly IUserModuleRepository _userModuleRepository;

        public UpdateUserModuleStatusHandler(IUserModuleRepository userModuleRepository)
        {
            _userModuleRepository = userModuleRepository;
        }

        public async Task<UserModule> Handle(UpdateUserModuleStatusCommand request, CancellationToken cancellationToken)
        {
            var userModule = await _userModuleRepository.FindByIdModuleWithIncludes(request.UserModuleId);
            userModule.State = request.State;
            await _userModuleRepository.Update(userModule);
            return userModule;
        }
    }
}

