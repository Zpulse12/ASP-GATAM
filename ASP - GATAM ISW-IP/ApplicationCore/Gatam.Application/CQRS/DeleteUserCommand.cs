using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Gatam.Application.CQRS
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
                .NotEmpty().WithMessage("User ID mag niet leeg zijn.");
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                {
                    var user = await _unitOfWork.UserRepository.FindById(userId);
                    return user != null;
                })
                .WithMessage("De gebruiker bestaat niet.");
        }
    }
    public class DeleteUserCommandHandler(IGenericRepository<ApplicationUser> userRepository, IValidator<DeleteUserCommand> validator)
        : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var user = await userRepository.FindById(request.UserId);
            if (user == null) return false;
            await userRepository.Delete(user);
            return true;
        }
    }


}
