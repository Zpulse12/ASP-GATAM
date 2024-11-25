﻿using FluentValidation;
using Gatam.Application.CQRS.User;
using Gatam.Application.Interfaces;
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
    public class DeleteModuleCommandHandler(IModuleRepository moduleRepository)
        : IRequestHandler<DeleteModuleCommand, bool>
    {
        public async Task<bool> Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
        {
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
