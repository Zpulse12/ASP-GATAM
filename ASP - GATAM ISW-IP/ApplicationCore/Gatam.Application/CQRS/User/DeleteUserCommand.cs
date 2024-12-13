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
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IManagementApi _managementApi;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(
            IManagementApi managementApi,
            IUnitOfWork unitOfWork)
        {
            _managementApi = managementApi;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.FindById(request.UserId);
            if (user == null)
            {
                return false;
            }
            var auth0Result = await _managementApi.DeleteUserAsync(request.UserId);
            if (!auth0Result)
            {
                return false;
            }

            if (user.UserRoles != null)
            {
                user.UserRoles.Clear();
            }

            await _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.Commit();

            return true;
        }
    }
}