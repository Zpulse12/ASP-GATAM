using AutoMapper;
using FluentValidation;
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
    public class DeactivateUserValidation : AbstractValidator<DeactivateUserCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeactivateUserValidation(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x._userId).NotEmpty()
                .WithMessage("user id cannot be empty");
            RuleFor(x => x._userId)
                .MustAsync(async (userId, token) =>
                {
                    var user = await _uow.UserRepository.FindById(userId);
                    return user != null;
                })
                .WithMessage("The user does not exist."); 
            RuleFor(x => x.IsActive)
                .Must(value => value == true || value == false)
                .WithMessage("IsActive must be either true or false.");
        }
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
