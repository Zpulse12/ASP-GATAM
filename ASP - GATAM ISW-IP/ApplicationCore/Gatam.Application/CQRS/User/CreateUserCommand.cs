using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;

        public CreateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _auth0Repository = auth0Repository;
        }
        public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.Create(request._user);
            await _unitOfWork.commit();
            var createUser = await _auth0Repository.CreateUserAsync(request._user);
            return _mapper.Map<ApplicationUser>(createUser);


        }
    }
}
