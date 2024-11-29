using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class GetUserModuleByUserQuery : IRequest<List<UserModuleDTO>>
    {
        public string UserId { get; set; }
    }
    public class GetUserModuleByUserQueryValidator : AbstractValidator<GetUserModuleByUserQuery>
    {
        public GetUserModuleByUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId mag niet leeg zijn.");
        }
    }
    public class GetUserModuleByUserQueryHandler : IRequestHandler<GetUserModuleByUserQuery, List<UserModuleDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserModuleByUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserModuleDTO>> Handle(GetUserModuleByUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserWithModules(request.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found");
            return _mapper.Map<List<UserModuleDTO>>(user.UserModules);
        }
    }
}
