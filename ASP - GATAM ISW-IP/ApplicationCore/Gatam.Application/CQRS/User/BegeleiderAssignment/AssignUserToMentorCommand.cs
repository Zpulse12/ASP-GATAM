using AutoMapper;
using FluentValidation;

using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;


namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class AssignUserToMentorCommand : IRequest<ApplicationUser>
    {
        public required string FollowerId { get; set; }
        public required string MentorId { get; set; }

    }

    public class AssignUserToMentorCommandValidator : AbstractValidator<AssignUserToMentorCommand>
    {
        public AssignUserToMentorCommandValidator()
        {
            RuleFor(x => x.FollowerId)
                .NotEmpty().WithMessage("VolgerId mag niet leeg zijn.");

            RuleFor(x => x.MentorId)
                .NotEmpty().WithMessage("BegeleiderId mag niet leeg zijn.");
        }
    }

    public class AssignUserToMentorCommandHandler : IRequestHandler<AssignUserToMentorCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AssignUserToMentorCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> Handle(AssignUserToMentorCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.FindById(request.FollowerId);
            user.MentorId = request.MentorId;
            await _uow.UserRepository.Update(user); 
            await _uow.Commit();
            return _mapper.Map<ApplicationUser>(user);
        }
    }

}
