using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.User
{
    public class GetBegeleiderForUserQuery : IRequest<UserDTO>
    {
        public string BegeleiderId { get; set; }

        public GetBegeleiderForUserQuery(string begeleiderId)
        {
            BegeleiderId = begeleiderId;
        }
        public class GetBegeleiderForUserQueryValidator : AbstractValidator<GetBegeleiderForUserQuery>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IManagementApi _managementApi;
            public GetBegeleiderForUserQueryValidator(IUnitOfWork unitOfWork, IManagementApi managementApi)
            {
                _unitOfWork = unitOfWork;
                _managementApi = managementApi;
                RuleFor(x => x.BegeleiderId)
                    .NotEmpty().WithMessage("Begeleider ID cannot be empty");
                RuleFor(x => x.BegeleiderId)
                    .MustAsync(async (userId, cancellation) =>
                    {
                        var user = await _unitOfWork.UserRepository.FindById(userId);
                        return user != null;
                    })
                    .WithMessage("The user doesn't exist");
            }
        }
        public class GetBegeleiderForUserQueryHandler : IRequestHandler<GetBegeleiderForUserQuery, UserDTO>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetBegeleiderForUserQueryHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<UserDTO> Handle(GetBegeleiderForUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _uow.UserRepository.FindById(request.BegeleiderId);
                var begeleider = await _uow.UserRepository.FindById(user.BegeleiderId);
                return _mapper.Map<UserDTO>(begeleider);
            }
        }
    }
}