using FluentValidation;
using MediatR;
using Gatam.Application.Interfaces;
using Gatam.Application.CQRS.DTOS;

namespace Gatam.Application.CQRS.User
{
    public class GetUsersWithModulesQuery : IRequest<List<UserDTO>> { }
    public class GetUsersModulesQueryValidator : AbstractValidator<GetUserModulesQuery>
    {
        public GetUsersModulesQueryValidator()
        {
            RuleFor(query => query.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");
        }
    }
    public class GetUsersWithModulesQueryHandler : IRequestHandler<GetUsersWithModulesQuery, List<UserDTO>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersWithModulesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserDTO>> Handle(GetUsersWithModulesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsersWithModulesAsync();
            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                Nickname = user.UserName,
                Email = user.Email,
                Modules = user.UserModules.Select(um => um.Module.Title)
                    .ToList(),
                RolesIds = null
            }).ToList();
        }
    }
}