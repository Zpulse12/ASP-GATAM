using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class DeactivateUserCommand : IRequest<UserDTO>
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
                .WithMessage("The user does not exist"); 
            RuleFor(x => x.IsActive)
                .Must(value => value == true || value == false)
                .WithMessage("IsActive must be either true or false");
        }
    }
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, UserDTO>
    {
        private readonly IManagementApi _managementApi;

        public DeactivateUserCommandHandler(IManagementApi managementApi)
        {
            _managementApi = managementApi;
        }

        public async Task<UserDTO> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var updatedUser = await _managementApi.UpdateUserStatusAsync(request._userId, request.IsActive);

            if (updatedUser == null)
            {
                return null;
            }

            return updatedUser;
        }
    }



}
