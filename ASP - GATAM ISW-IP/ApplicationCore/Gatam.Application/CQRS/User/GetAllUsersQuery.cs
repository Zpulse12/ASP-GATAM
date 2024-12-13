using AutoMapper;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetAllUsersQuery : IRequest<List<UserDTO>>
    {
    }
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;
        private readonly EnvironmentWrapper _environmentWrapper;

        public GetAllUsersQueryHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository, EnvironmentWrapper environmentWrapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _auth0Repository = auth0Repository;
            _environmentWrapper = environmentWrapper;
        }

        public async Task<List<UserDTO>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync(x => x.UserRoles);
            return _mapper.Map<List<UserDTO>>(users);
        }

    }
}
