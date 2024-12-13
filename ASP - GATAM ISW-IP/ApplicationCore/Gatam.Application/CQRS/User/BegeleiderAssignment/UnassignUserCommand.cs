using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class UnassignUserCommand : IRequest<UserDTO>
    {
        public required string FollowerId { get; set; }
    }

    public class UnassignUserCommandValidator : AbstractValidator<UnassignUserCommand>
    {
        public UnassignUserCommandValidator()
        {
            RuleFor(x => x.FollowerId)
                .NotEmpty().WithMessage("VolgerId mag niet leeg zijn");
        }
    }

    public class UnassignUserCommandHandler : IRequestHandler<UnassignUserCommand, UserDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UnassignUserCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<UserDTO> Handle(UnassignUserCommand request, CancellationToken cancellationToken)
        {
            var userEntity = await _uow.UserRepository.FindById(request.FollowerId);
            userEntity.MentorId = null;
            await _uow.UserRepository.Update(userEntity);
            await _uow.Commit();
            return _mapper.Map<UserDTO>(userEntity);
        }
    }
}
