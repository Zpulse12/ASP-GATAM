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
        _unitOfWork = unitOfWork;
        RuleFor(x => x.User.Username)
           .NotEmpty().WithMessage("Nickname mag niet leeg zijn")
           .MustAsync(async (userCommand, nickname, cancellationToken) =>
           {
               var existingUser = await _unitOfWork.UserRepository.FindByProperty("Nickname", nickname);
               return existingUser == null || existingUser.Id == userCommand.Id;
           }).WithMessage("Username bestaat al");

        RuleFor(x => x.User.Email)
           .NotEmpty().WithMessage("Email mag niet leeg zijn")
           .EmailAddress().WithMessage("Ongeldig e-mailformaat")
           .MustAsync(async (userCommand, email, cancellationToken) =>
           {
               var existingUser = await _unitOfWork.UserRepository.FindByProperty("Email", email);
               return existingUser == null || existingUser.Id == userCommand.Id;
           }).WithMessage("Email bestaat al.");
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
        var user = await _uow.UserRepository.FindById(request.Id);

        user.Email = request.User.Email;
        user.Username = request.User.Username;

        await _uow.UserRepository.Update(user);
        await _uow.Commit();

        return _mapper.Map<UserDTO>(user);
    }
}
