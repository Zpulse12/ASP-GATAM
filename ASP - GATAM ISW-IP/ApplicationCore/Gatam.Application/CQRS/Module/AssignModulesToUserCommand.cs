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
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AssignModulesToUserCommandHandler(
        IModuleRepository moduleRepository, 
        IUnitOfWork uow,
        IMapper mapper)
    {
        _moduleRepository = moduleRepository;
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<UserModuleDTO> Handle(AssignModulesToUserCommand request, CancellationToken cancellationToken)
    {
        var follower = await _uow.UserRepository.FindById(request.FollowerId);
        var moduleTemplate = await _moduleRepository.FindByIdWithQuestions(request.ModuleId);
        
        var newUserModule = new UserModule 
        { 
            UserId = follower.Id, 
            ModuleId = moduleTemplate.Id,
            User = follower,
            Module = moduleTemplate,
            UserQuestions = moduleTemplate.Questions.Select(templateQuestion => new UserQuestion
            {
                QuestionId = templateQuestion.Id,
                IsVisible = true
            }).ToList()
        };
        
        follower.UserModules.Add(newUserModule);
        await _uow.UserRepository.Update(follower);
        await _uow.Commit();

        return _mapper.Map<UserModuleDTO>(newUserModule);
    }
}