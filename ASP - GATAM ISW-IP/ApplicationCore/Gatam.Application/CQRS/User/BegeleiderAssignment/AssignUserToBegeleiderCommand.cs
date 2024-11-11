using AutoMapper;
using FluentValidation;
using Gatam.Application.CQRS.Module;
using Gatam.Application.CQRS.User.Roles;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class AssignUserToBegeleiderCommand : IRequest<ApplicationUser>
    {
        public required string VolgerId { get; set; }
        public required string BegeleiderId { get; set; }

    }

    public class AssignUserToBegeleiderCommandValidator : AbstractValidator<AssignUserToBegeleiderCommand>
    {
        public AssignUserToBegeleiderCommandValidator()
        {
            RuleFor(x => x.VolgerId)
                .NotEmpty().WithMessage("VolgerId cannot be empty");

            RuleFor(x => x.BegeleiderId)
                .NotEmpty().WithMessage("BegeleiderId cannot be empty");
        }
    }

    public class AssignUserToBegeleiderCommandHandler : IRequestHandler<AssignUserToBegeleiderCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AssignUserToBegeleiderCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> Handle(AssignUserToBegeleiderCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.FindById(request.VolgerId);
            user.BegeleiderId = request.BegeleiderId;
            await _uow.UserRepository.Update(user); 
            await _uow.commit();
            return _mapper.Map<ApplicationUser>(user);
        }
    }

}
