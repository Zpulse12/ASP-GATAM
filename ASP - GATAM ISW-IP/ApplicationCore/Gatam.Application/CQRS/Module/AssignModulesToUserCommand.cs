using FluentValidation;
using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;

namespace Gatam.Application.CQRS.Module;

public class AssignModulesToUserCommand : IRequest<UserModule>
{
    public string VolgerId { get; set; }
    public string ModuleId { get; set; }
}
public class AssignModulesToUserCommandValidator : AbstractValidator<AssignModulesToUserCommand>
{        
    private readonly IUnitOfWork _uow;
    public AssignModulesToUserCommandValidator(IUnitOfWork uow)
    {
        _uow = uow;
        RuleFor(command => command.VolgerId)
            .NotEmpty()
            .WithMessage("VolgerId is required.");

        RuleFor(command => command.ModuleId)
            .NotEmpty()
            .WithMessage("ModuleId is required.");
        RuleFor(x => x.VolgerId)
            .MustAsync(async (userId, token) =>
            {
                var user = await _uow.UserRepository.FindById(userId);
                return user != null;
            })
            .WithMessage("The user does not exist"); 
        RuleFor(x => x.ModuleId)
            .MustAsync(async (moduleId, token) =>
            {
                var user = await _uow.ModuleRepository.FindById(moduleId);
                return user != null;
            })
            .WithMessage("The module does not exist"); 
        RuleFor(command => command)
            .MustAsync(async (command, cancellation) =>
            {
                var user = await _uow.UserRepository.FindById(command.VolgerId);
                return user != null && user.UserModules.All(um => um.ModuleId != command.ModuleId);
            })
            .WithMessage("Module is already assigned to this user");
    }
}
public class AssignModulesToUserCommandHandler : IRequestHandler<AssignModulesToUserCommand, UserModule>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignModulesToUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserModule> Handle(AssignModulesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.FindById(request.VolgerId);
        var module = await _unitOfWork.ModuleRepository.FindByIdModuleWithIncludes(request.ModuleId);

        var userModule = new UserModule
        {
            UserId = user.Id,
            ModuleId = module.Id,
            State = UserModuleState.NotStarted
        };

        await _unitOfWork.UserModuleRepository.Create(userModule);

        foreach (var question in module.Questions)
        {
            foreach (var answer in question.Answers)
            {
                var userAnswer = new UserAnswer
                {
                    UserModuleId = userModule.Id, 
                    QuestionAnswerId = answer.Id,
                    GivenAnswer = string.Empty,
                };

                if (userModule.UserGivenAnswers == null)
                {
                    userModule.UserGivenAnswers = new List<UserAnswer>();
                }
                userModule.UserGivenAnswers.Add(userAnswer);

                await _unitOfWork.UserAnwserRepository.Create(userAnswer);
            }
        }

        await _unitOfWork.Commit();
        return userModule;
    }
}