using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public required string UserId { get; set; }
    }
    public class DeleteUserCommandHandler(IGenericRepository<ApplicationUser> userRepository)
        : IRequestHandler<DeleteUserCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.FindById(request.UserId);
            if (user == null) return false;
            userRepository.Delete(user);
            return true;
        }
    }


}
