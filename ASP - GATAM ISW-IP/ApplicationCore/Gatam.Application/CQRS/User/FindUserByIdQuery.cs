using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User;

public class FindUserByIdQuery : IRequest<ApplicationUser>
{
    public string Auth0UserId { get; set; }
    
    public FindUserByIdQuery(string auth0UserId)
    {
        Auth0UserId = auth0UserId;
    }
    public class FindUserByIdQueryHandler : IRequestHandler<FindUserByIdQuery, ApplicationUser>
    {
        private readonly IUnitOfWork _uow;

        public FindUserByIdQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ApplicationUser> Handle(FindUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _uow.UserRepository.FindById(request.Auth0UserId);
        }
    }

}
