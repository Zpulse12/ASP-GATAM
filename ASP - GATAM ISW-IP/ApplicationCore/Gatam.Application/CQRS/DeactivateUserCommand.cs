using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;


namespace Gatam.Application.CQRS
{
    public class DeactivateUserCommand : IRequest<IEnumerable<UserDTO>>
    {
        public required string _userId { get; set; }
        public bool IsActive { get; set; }
    }

    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, IEnumerable<UserDTO>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public DeactivateUserCommandHandler()
        {
        }

        public DeactivateUserCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<UserDTO>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await uow.UserRepository.FindById(request._userId);
            if (user == null)
            {
                return null;
            }
            user.IsActive = request.IsActive;
            _=uow.UserRepository.Update(user);
            await uow.commit();

            var userDTO = mapper.Map<UserDTO>(user);
            return [userDTO];
        }
    }


}
