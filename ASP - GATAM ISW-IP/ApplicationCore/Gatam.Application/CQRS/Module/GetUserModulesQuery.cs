using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User;

    public class GetUserModulesQuery : IRequest<List<ModuleDTO>>
    {
        public string UserId { get; set; }

        public GetUserModulesQuery(string userId)
        {
            UserId = userId;
        }
    }

    public class GetUserModulesQueryValidator : AbstractValidator<GetUserModulesQuery>
    {
        private readonly IUnitOfWork _uow;
        public GetUserModulesQueryValidator(IUnitOfWork uow)
        {
            _uow = uow;
            RuleFor(query => query.UserId)
            .NotEmpty()
            .WithMessage("UserId mag niet leeg zijn.");
        RuleFor(x => x.UserId).MustAsync(async (userId, CancellationToken) =>
        {
            var user = await _uow.UserRepository.FindById(userId);
            return user != null;
        }).WithMessage("User bestaat niet");
        }
    }
    public class GetUserModulesQueryHandler : IRequestHandler<GetUserModulesQuery, List<ModuleDTO>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

    public GetUserModulesQueryHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;

    }

    public async Task<List<ModuleDTO>> Handle(GetUserModulesQuery request, CancellationToken cancellationToken)
    {
        var user = await _uow.UserRepository.GetUserWithModules(request.UserId);
        return _mapper.Map<List<ModuleDTO>>(user.UserModules.Select(um => um.Module).ToList());
    }
}