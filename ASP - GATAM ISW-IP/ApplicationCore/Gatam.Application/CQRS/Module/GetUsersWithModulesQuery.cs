using FluentValidation;
using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using AutoMapper;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Application.CQRS.DTOS.ModulesDTO;

namespace Gatam.Application.CQRS.Module
{
    public class GetUsersWithModulesQuery : IRequest<List<UserModuleDTO>> { }
    public class GetUsersModulesQueryValidator : AbstractValidator<GetUserModulesQuery>
    {
        public GetUsersModulesQueryValidator()
        {
            RuleFor(query => query.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
    public class GetUsersWithModulesQueryHandler : IRequestHandler<GetUsersWithModulesQuery, List<UserModuleDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersWithModulesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserModuleDTO>> Handle(GetUsersWithModulesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsersWithModulesAsync();
            
            return users.SelectMany(u => u.UserModules.Select(um => new UserModuleDTO
            {
                Id = um.Id,
                User = new UserDTO 
                { 
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email 
                },
                Module = new ModuleDTO
                {
                    Id = um.Module.Id,
                    Title = um.Module.Title,
                    Category = um.Module.Category
                },
                QuestionSettings = um.QuestionSettings.Select(qs => new QuestionSettingDTO
                {
                    Id = qs.Id,
                    IsVisible = qs.IsVisible,
                    Question = new QuestionDTO
                    {
                        Id = qs.Question.Id,
                        QuestionTitle = qs.Question.QuestionTitle,
                        QuestionType = qs.Question.QuestionType
                    }
                }).ToList()
            })).ToList();
        }
    }
}