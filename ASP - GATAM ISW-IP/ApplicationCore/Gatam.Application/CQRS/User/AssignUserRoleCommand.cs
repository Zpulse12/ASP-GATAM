using AutoMapper;
using FluentValidation;
using Gatam.Application.Extensions;
using Gatam.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.User
{
    public class AssignUserRoleCommand : IRequest<UserDTO>
    {
        public string Id { get; set; }
        public required UserDTO User { get; set; }
    }

    public class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignUserRoleCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.User.Id)
                .NotEmpty().WithMessage("Id cannot be empty")
                .Equal(x => x.Id)
                .WithMessage("Id does not equal given userId");
            RuleFor(x => x.User.RolesIds)
                .NotEmpty().WithMessage("Role cannot be empty")
                .NotNull().WithMessage("Role cannot be null");
        }
    }

    public class AssignUserRoleCommandHandler : IRequestHandler<AssignUserRoleCommand, UserDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;

        public AssignUserRoleCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository)
        {
            _uow = uow;
            _mapper = mapper;
            _auth0Repository = auth0Repository;
        }

        public async Task<UserDTO> Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {


            if (request.User.RolesIds != null && request.User.RolesIds.Any())
            {

                await _auth0Repository.UpdateUserRoleAsync(request.User);

            }

            return _mapper.Map<UserDTO>(request.User);

        }

    }
}
