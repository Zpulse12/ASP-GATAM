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
        private readonly IUnitOfWork _uow;

        public GetUserModuleByUserQueryValidator(IUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId mag niet leeg zijn.");
            RuleFor(x => x.UserId).MustAsync(async (userId, CancellationToken) =>
            {
                var user = await _uow.UserRepository.FindById(userId);
                return user != null;
            }).WithMessage("User bestaat niet");
        }
    }
    public class GetUserModuleByUserQueryHandler : IRequestHandler<GetUserModuleByUserQuery, List<UserModuleDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserModuleByUserQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<UserModuleDTO>> Handle(GetUserModuleByUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetUserWithModules(request.UserId);
            return _mapper.Map<List<UserModuleDTO>>(user.UserModules);
        }
    }
}
