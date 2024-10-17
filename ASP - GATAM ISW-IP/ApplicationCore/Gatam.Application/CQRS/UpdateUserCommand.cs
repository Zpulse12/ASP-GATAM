using System.ComponentModel.DataAnnotations;
using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Gatam.Application.CQRS;

public class UpdateUserCommand:IRequest<UserDTO>
{
    public string Id { get; set; }
    public required UserDTO User { get; set; }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        RuleFor(x => x.User.Username)
            .NotEmpty().WithMessage("Username cannot be empty")
            .MustAsync(async (userCommand, username, cancellationToken) =>
            {
                var existingUser = await _unitOfWork.UserRepository.FindByProperty("UserName", username);
                return existingUser == null || existingUser.Id == userCommand.Id;
            }).WithMessage("Username already exists.");

        RuleFor(x => x.User.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email format")
            .MustAsync(async (userCommand, email, cancellationToken) =>
            {
                var existingUser = await _unitOfWork.UserRepository.FindByProperty("Email", email);
                return existingUser == null || existingUser.Id == userCommand.Id;
            }).WithMessage("Email already exists.");

    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateUserCommand> _validator;


    public UpdateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IValidator<UpdateUserCommand> validator)
    {
        _uow = uow;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validationCheck = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationCheck.IsValid)
        {
            throw new ValidationException(validationCheck.Errors);
        }

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
