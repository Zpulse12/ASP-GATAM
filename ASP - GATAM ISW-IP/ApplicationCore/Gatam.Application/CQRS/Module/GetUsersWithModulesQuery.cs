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
        public GetUsersModulesQueryValidator(IUnitOfWork uow)
        {
           
        }
    }
    public class GetUsersWithModulesQueryHandler : IRequestHandler<GetUsersWithModulesQuery, List<UserModuleDTO>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersWithModulesQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<UserModuleDTO>> Handle(GetUsersWithModulesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetUsersWithModulesAsync();
            
            var userModules = users.SelectMany(u => u.UserModules).ToList();
            return _mapper.Map<List<UserModuleDTO>>(userModules);
        }
    }
}