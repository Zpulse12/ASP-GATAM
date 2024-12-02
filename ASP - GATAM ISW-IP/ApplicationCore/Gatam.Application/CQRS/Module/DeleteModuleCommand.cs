using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module
{
    public class DeleteModuleCommand : IRequest<bool>
    {
        public required string ModuleId { get; set; }
    }
    public class DeleteModuleCommandValidator : AbstractValidator<DeleteModuleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public DeleteModuleCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            RuleFor(x => x.ModuleId)
                           .NotEmpty().WithMessage("Module ID kan niet leeg zijn");
            RuleFor(x => x.ModuleId)
                .MustAsync(async (moduleId, cancellation) =>
                {
                    var module = await _unitOfWork.ModuleRepository.FindByIdWithQuestions(moduleId);
                    return module != null;
                })
                .WithMessage("Module bestaat niet");
            RuleFor(x => x.ModuleId)
            .MustAsync(async (moduleId, cancellation) =>
            {
                var usersWithModule = await _unitOfWork.UserRepository.GetUsersByModuleIdAsync(moduleId);


                return !usersWithModule.Any();
            })
            .WithMessage("De module kan niet worden verwijderd omdat deze is toegewezen aan een gebruiker.");
        }
    }
    public class DeleteModuleCommandHandler: IRequestHandler<DeleteModuleCommand, bool>
    {
        private readonly IUnitOfWork _uow;
       

        public DeleteModuleCommandHandler(IUnitOfWork uow)
        {
            this._uow = uow;
           
        }

        public async Task<bool> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {

            var module = await _uow.ModuleRepository.FindByIdWithQuestions(request.ModuleId);
            try
            {
                await _uow.ModuleRepository.Delete(module);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
