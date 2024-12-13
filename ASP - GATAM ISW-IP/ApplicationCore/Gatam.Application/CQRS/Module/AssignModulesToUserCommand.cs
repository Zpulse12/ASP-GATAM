using FluentValidation;
using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using AutoMapper;
using Gatam.Application.CQRS.DTOS.ModulesDTO;

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
            .WithMessage("VolgerID mag niet leeg zijn.");

        RuleFor(command => command.ModuleId)
            .NotEmpty()
            .WithMessage("ModuleId mag niet leeg zijn.");

        RuleFor(x => x.FollowerId)
            .MustAsync(async (userId, token) =>
            {
                var user = await _uow.UserRepository.FindById(userId);
                return user != null;
            })
            .WithMessage("De gebruiker bestaat niet."); 

        RuleFor(x => x.ModuleId)
            .MustAsync(async (moduleId, token) =>
            {
                var module = await _uow.ModuleRepository.FindById(moduleId);
                return module != null;
            })
            .WithMessage("De module bestaat niet."); 

        RuleFor(command => command)
            .MustAsync(async (command, cancellation) =>
            {
                var user = await _uow.UserRepository.FindById(command.FollowerId);
                return user != null && !user.UserModules.Any(um => um.ModuleId == command.ModuleId);
            })
            .WithMessage("Deze module is al toegewezen aan deze volger.");
    }
}
public class AssignModulesToUserCommandHandler : IRequestHandler<AssignModulesToUserCommand, UserModuleDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AssignModulesToUserCommandHandler(
        IUnitOfWork uow,
        IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }
    
    public async Task<UserModuleDTO> Handle(AssignModulesToUserCommand request, CancellationToken cancellationToken)
    {
        var follower = await _uow.UserRepository.FindById(request.FollowerId);
        var moduleTemplate = await _uow.ModuleRepository.FindByIdWithQuestions(request.ModuleId);

        var newUserModule = new UserModule
        {
            UserId = follower.Id,
            ModuleId = moduleTemplate.Id,
            User = follower,
            Module = moduleTemplate,
            UserQuestions = moduleTemplate.Questions.Select(templateQuestion => new UserQuestion
            {
                QuestionId = templateQuestion.Id,
                IsVisible = true,
                QuestionPriority = QuestionPriority.HIGH
            }).ToList()
        };

        follower.UserModules.Add(newUserModule);
        await _uow.UserRepository.Update(follower);

        foreach (var question in moduleTemplate.Questions)
        {
            foreach (var answer in question.Answers)
            {
                var userAnswer = new UserAnswer
                {
                    UserModuleId = newUserModule.Id, 
                    QuestionAnswerId = answer.Id,
                    GivenAnswer = string.Empty,
                };

                if (newUserModule.UserGivenAnswers == null)
                {
                    newUserModule.UserGivenAnswers = new List<UserAnswer>();
                }
                newUserModule.UserGivenAnswers.Add(userAnswer);

                await _uow.UserAnwserRepository.Create(userAnswer);
            }
        }

        await _uow.Commit();

        return _mapper.Map<UserModuleDTO>(newUserModule);
    }
}