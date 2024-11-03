using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class CreateUserCommand: IRequest<ApplicationUser>
    {
        public required ApplicationUser _user { get; set; }


    }
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            await _unitOfWork.UserRepository.Create(request._user);
            await _unitOfWork.commit();
            return request._user;
        }
    }
}
