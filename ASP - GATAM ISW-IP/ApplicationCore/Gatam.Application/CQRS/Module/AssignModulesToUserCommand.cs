using FluentValidation;
using Gatam.Application.CQRS.Questions;
using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using AutoMapper;

namespace Gatam.Application.CQRS.Module;

public class AssignModulesToUserCommand : IRequest<UserModuleDTO>
{
    public string FollowerId { get; set; }
    public string ModuleId { get; set; }
}
public class AssignModulesToUserCommandValidator : AbstractValidator<AssignModulesToUserCommand>
{        
    private readonly IUnitOfWork _uow;
    public AssignModulesToUserCommandValidator(IUnitOfWork uow)
    {
        _uow = uow;
        
        RuleFor(command => command.FollowerId)
            .NotEmpty()
            .WithMessage("FollowerId is required.");

        RuleFor(command => command.ModuleId)
            .NotEmpty()
            .WithMessage("ModuleId is required.");

        RuleFor(x => x.FollowerId)
            .MustAsync(async (userId, token) =>
            {
                var user = await _uow.UserRepository.FindById(userId);
                return user != null;
            })
            .WithMessage("The user does not exist"); 

        RuleFor(x => x.ModuleId)
            .MustAsync(async (moduleId, token) =>
            {
                var module = await _uow.ModuleRepository.FindById(moduleId);
                return module != null;
            })
            .WithMessage("The module does not exist"); 

        // Single validation for duplicate assignment
        RuleFor(command => command)
            .MustAsync(async (command, cancellation) =>
            {
                var user = await _uow.UserRepository.FindById(command.FollowerId);
                return user != null && !user.UserModules.Any(um => um.ModuleId == command.ModuleId);
            })
            .WithMessage("Module is already assigned to this user");
    }
}
public class AssignModulesToUserCommandHandler : IRequestHandler<AssignModulesToUserCommand, UserModuleDTO>
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AssignModulesToUserCommandHandler(
        IModuleRepository moduleRepository, 
        IMediator mediator, 
        IUnitOfWork uow,
        IMapper mapper)
    {
        _moduleRepository = moduleRepository;
        _mediator = mediator;
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<UserModuleDTO> Handle(AssignModulesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _uow.UserRepository.FindById(request.FollowerId);
        var module = await _moduleRepository.FindByIdWithQuestions(request.ModuleId);
        
        var userModule = new UserModule 
        { 
            UserId = user.Id, 
            ModuleId = module.Id,
            User = user,
            Module = module
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

        return _mapper.Map<UserModuleDTO>(userModule);
    }
}