using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS;

public class UpdateUserCommand:IRequest<UserDTO>
{
    public string Id { get; set; }
    public required UserDTO User { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;    
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var person = await _uow.UserRepository.FindById(request.Id);

        if (person == null)
        {
            throw new Exception($"User with ID {request.Id} was not found.");
        }
        _mapper.Map(request.User, person); 
        var updatedPerson = await _uow.UserRepository.Update(person);
        await _uow.commit(); 
        return _mapper.Map<UserDTO>(updatedPerson);
    }
}
