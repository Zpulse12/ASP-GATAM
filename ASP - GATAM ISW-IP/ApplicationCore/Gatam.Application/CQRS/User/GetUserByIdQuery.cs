using Gatam.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.User
{
    public class GetUserByIdQuery: IRequest<UserDTO>
    {
        public string UserId { get; set; }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO>
    {
        private readonly IManagementApi _auth0Repository;


        public GetUserByIdQueryHandler(IManagementApi auth0Repository)
        {
            _auth0Repository = auth0Repository;
        }

        public async Task<UserDTO> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _auth0Repository.GetUserByIdAsync(request.UserId);
        }

    }
}
