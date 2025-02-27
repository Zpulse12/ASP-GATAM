﻿using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class UpdateUserModuleStatusCommand : IRequest<UserModule>
    {
        public string UserModuleId { get; set; }
        public UserModuleState State { get; set; }
    }
    public class UpdateUserModuleStatusCommandValidator: AbstractValidator<UpdateUserModuleStatusCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateUserModuleStatusCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.UserModuleId)
                .NotEmpty().WithMessage("UserModuleId mag niet leeg zijn.")
                .MustAsync(async (userModuleId, cancellationToken) => await UserModuleExists(userModuleId))
                .WithMessage("UserModuleId bestaat niet.");
            RuleFor(x => x.UserModuleId).NotEmpty().WithMessage("UserModuleId mag niet leeg zijn");
            RuleFor(x => x.State).NotNull().WithMessage("State mag niet null zijn");
        }
        private async Task<bool> UserModuleExists(string userModuleId)
        {
            var userModule = await _uow.UserModuleRepository.FindById(userModuleId);
            return userModule != null;
        }
    }
    public class UpdateUserModuleStatusHandler : IRequestHandler<UpdateUserModuleStatusCommand, UserModule>
    {
        private readonly IUnitOfWork _uow;

        public UpdateUserModuleStatusHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<UserModule> Handle(UpdateUserModuleStatusCommand request, CancellationToken cancellationToken)
        {
            var userModule = await _uow.UserModuleRepository.FindByIdModuleWithIncludes(request.UserModuleId);
            
            var visibleQuestionsCount = userModule.UserQuestions.Where(q => q.IsVisible).Count();
            var answeredQuestionsCount = userModule.UserGivenAnswers
                .Where(a => !string.IsNullOrEmpty(a.GivenAnswer))
                .Select(a => a.QuestionAnswer.QuestionId)
                .Distinct()
                .Count();

            if (answeredQuestionsCount > 0 && answeredQuestionsCount < visibleQuestionsCount)
            {
                userModule.State = UserModuleState.InProgress;
            }
            else if (answeredQuestionsCount >= visibleQuestionsCount)
            {
                userModule.State = UserModuleState.Done;
            }

            await _uow.UserModuleRepository.Update(userModule);
            await _uow.Commit(); 
            
            return userModule;
        }
    }
}

