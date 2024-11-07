using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
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
        public DeleteUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID cannot be empty");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await _unitOfWork.UserRepository.FindById(userId);
                    return user != null;
                })
                .WithMessage("The user doesnt exist");
        }
    }
    public class DeleteUserCommandHandler(IGenericRepository<ApplicationUser> userRepository, IManagementApi managementApi)
        : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var auth0User = await managementApi.GetUserByIdAsync(request.UserId);
            var user = await userRepository.FindById(request.UserId);
            if (auth0User != null && user != null)
            {
                var result = await managementApi.DeleteUserAsync(request.UserId);
                if (result)
                {
                        await userRepository.Delete(user);
                        return true;
                }
            }
            return false;
        }
    }
}