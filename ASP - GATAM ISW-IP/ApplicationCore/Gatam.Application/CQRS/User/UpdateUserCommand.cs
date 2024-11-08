using AutoMapper;
using FluentValidation;
using Gatam.Application.Interfaces;
using MediatR;

namespace Gatam.Application.CQRS.User;

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
        //_unitOfWork = unitOfWork;
        //RuleFor(x => x.User.Nickname)
        //    .NotEmpty().WithMessage("Nickname cannot be empty")
        //    .MustAsync(async (userCommand, nickname, cancellationToken) =>
        //    {
        //        var existingUser = await _unitOfWork.UserRepository.FindByProperty("Nickname", nickname);
        //        return existingUser == null || existingUser.Id == userCommand.Id;
        //    }).WithMessage("Username already exists.");

        //RuleFor(x => x.User.Email)
        //    .NotEmpty().WithMessage("Email cannot be empty")
        //    .EmailAddress().WithMessage("Invalid email format")
        //    .MustAsync(async (userCommand, email, cancellationToken) =>
        //    {
        //        var existingUser = await _unitOfWork.UserRepository.FindByProperty("Email", email);
        //        return existingUser == null || existingUser.Id == userCommand.Id;
        //    }).WithMessage("Email already exists.");
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDTO>
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IManagementApi _auth0Repository;

    public UpdateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository)
    {
        _uow = uow;
        _mapper = mapper;
        _auth0Repository = auth0Repository;
    }

    public async Task<UserDTO> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
            await _auth0Repository.UpdateUserNicknameAsync(request.User);
            await _auth0Repository.UpdateUserEmailAsync(request.User);
      
            return request.User;
    }

    
}
