using FluentValidation;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
    }
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IManagementApi _managementApi;
        public DeleteUserCommandValidator(IUnitOfWork unitOfWork, IManagementApi managementApi)
        {
            _unitOfWork = unitOfWork;
            _managementApi = managementApi;
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID cannot be empty");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await _unitOfWork.UserRepository.FindById(userId);
                    return user != null;
                })
                .WithMessage("The user doesnt exist");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await _managementApi.GetUserByIdAsync(userId);
                    return user != null;
                })
                .WithMessage("The user doesnt exist");
        }
    }
    public class DeleteUserCommandHandler(IUserRepository userRepository, IManagementApi managementApi)
        : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindById(request.UserId);
            var result = await managementApi.DeleteUserAsync(request.UserId);
            if (result)
            {
                await userRepository.Delete(user);
                return true;
            }
            return false;
        }
    }
}