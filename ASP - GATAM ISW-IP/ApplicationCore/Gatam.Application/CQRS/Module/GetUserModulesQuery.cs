using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User;

    public class GetUserModulesQuery : IRequest<List<ApplicationModule>>
    {
        public string UserId { get; set; }

        public GetUserModulesQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserModulesQueryValidator : AbstractValidator<GetUserModulesQuery>
    {
        public GetUserModulesQueryValidator()
        {
        RuleFor(query => query.UserId)
            .NotEmpty()
            .WithMessage("UserId mag niet leeg zijn.");
        }
    }
    public class GetUserModulesQueryHandler : IRequestHandler<GetUserModulesQuery, List<ApplicationModule>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

    public GetUserModulesQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<ApplicationModule>> Handle(GetUserModulesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserWithModules(request.UserId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        return _mapper.Map<List<ApplicationModule>>(user.UserModules.Select(um => um.Module).ToList());
    }
}