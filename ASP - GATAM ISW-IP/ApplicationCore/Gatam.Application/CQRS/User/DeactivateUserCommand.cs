using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class DeactivateUserCommand : IRequest<UserDTO>
    {
        public required string UserId { get; set; }
        public bool IsActive { get; set; }
    }
    public class DeactivateUserValidation : AbstractValidator<DeactivateUserCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeactivateUserValidation(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.UserId).NotEmpty()
                .WithMessage("user id cannot be empty");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, token) =>
                {
                    var user = await _uow.UserRepository.FindById(userId);
                    return user != null;
                })
                .WithMessage("The user does not exist"); 
            RuleFor(x => x.IsActive)
                .Must(value => value == true || value == false)
                .WithMessage("IsActive must be either true or false");
        }
    }
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, UserDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public DeactivateUserCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }
        public async Task<UserDTO> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.FindById(request.UserId);
            if (user == null)
            {
                return null;
            }
            user.IsActive = request.IsActive; 
            await _uow.UserRepository.Update(user);
            await _uow.Commit();

            return _mapper.Map<UserDTO>(user);
        }
    }




}
