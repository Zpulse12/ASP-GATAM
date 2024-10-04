using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class CreateUserCommand: IRequest<ApplicationUser>
    {
        public required ApplicationUser _user { get; set; }

    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApplicationUser>
    {
        private readonly IUnitOfWork uow;

        public CreateUserCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<ApplicationUser> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            await uow.UserRepository.Create(request._user);
            await uow.commit();
            return request._user;
        }
    }
}
