using FluentValidation;
using Gatam.Application.CQRS.Questions;
using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;

namespace Gatam.Application.CQRS.Module;

public class AssignModulesToUserCommand : IRequest<bool>
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
public class AssignModulesToUserCommandHandler : IRequestHandler<AssignModulesToUserCommand, bool>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _uow;

    public AssignModulesToUserCommandHandler(IModuleRepository moduleRepository, IMediator mediator, IUnitOfWork uow)
    {
        _moduleRepository = moduleRepository;
        _mediator = mediator;
        _uow = uow;
    }
    
    public async Task<bool> Handle(AssignModulesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.UserRepository.FindById(request.VolgerId);
        var module = await _moduleRepository.FindByIdWithQuestions(request.ModuleId);
        var userModule = new UserModule 
        { 
            UserId = user.Id, 
            ModuleId = module.Id 
        };
        user.UserModules.Add(userModule);
        
        await _uow.UserRepository.Update(user);
        await _uow.Commit();

        foreach (var question in module.Questions)
        {
            await _mediator.Send(new CreateSettingCommand
            {
                UserModuleId = userModule.Id,
                QuestionId = question.Id
            });
        }

        return true;
    }
}