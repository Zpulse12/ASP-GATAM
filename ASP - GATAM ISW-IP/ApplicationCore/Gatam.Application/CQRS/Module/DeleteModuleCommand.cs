using FluentValidation;
using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
 
        }
    }
    public class DeleteModuleCommandHandler(IModuleRepository moduleRepository, IUserRepository userRepository) : IRequestHandler<DeleteModuleCommand, bool>
    {
        public async Task<bool> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {

            var usersWithModule = await userRepository.GetUsersByModuleIdAsync(request.ModuleId);

            if (usersWithModule.SelectMany(u => u.UserModules)
                               .Any(um => um.ModuleId == request.ModuleId && um.State == UserModuleState.InProgress))
            {
                throw new InvalidOperationException("De module kan niet worden verwijderd omdat deze in gebruik is door een trajectvolger.");
            }
            var module = await moduleRepository.FindByIdWithQuestions(request.ModuleId);
            if (module != null)
            {
                await moduleRepository.Delete(module);
                return true;
            }
            return false;
        }
    }
}
