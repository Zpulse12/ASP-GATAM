﻿using FluentValidation;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class SubmitUserAnswersCommand : IRequest<List<UserAnswer>>
    {
        public string UserModuleId { get; set; }
        public List<UserAnswer> UserAnwsers { get; set; }
    }
    public class SubmitUserAnswersCommandValidator : AbstractValidator<SubmitUserAnswersCommand>
    {
        public SubmitUserAnswersCommandValidator()
        {
            RuleFor(x => x.UserModuleId)
                .NotEmpty().WithMessage("UserModuleId mag niet leeg zijn.")
                .WithMessage("UserModuleId is ongeldig.");

            RuleFor(x => x.UserAnwsers)
                .NotNull().WithMessage("UserAnwsers mag niet null zijn.");
        }
    }
        public class SubmitUserAnswersCommandHandler : IRequestHandler<SubmitUserAnswersCommand, List<UserAnswer>>
    {
        private readonly IUnitOfWork _uow;
        public SubmitUserAnswersCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<UserAnswer>> Handle(SubmitUserAnswersCommand request, CancellationToken cancellationToken)
        {
            var userModule = await _uow.UserModuleRepository.FindById(request.UserModuleId);

            var existingAnswers = userModule.UserGivenAnswers;
            var newAnswers = request.UserAnwsers;

            var answersToUpdate = new List<UserAnswer>();
            var answersToAdd = new List<UserAnswer>();

            foreach (var newAnswer in newAnswers)
            {
                var existingAnswer = await _uow.UserAnwserRepository.FindById(newAnswer.Id);

                if (existingAnswer != null)
                {
                    if (existingAnswer.GivenAnswer != newAnswer.GivenAnswer)
                    {
                        existingAnswer.GivenAnswer = newAnswer.GivenAnswer;
                        answersToUpdate.Add(existingAnswer);
                    }
                }
                else
                {
                    var answerToAdd = new UserAnswer
                    {
                        Id = newAnswer.Id,
                        UserModuleId = request.UserModuleId,
                        QuestionAnswerId = newAnswer.QuestionAnswerId,
                        GivenAnswer = newAnswer.GivenAnswer
                    };
                    answersToAdd.Add(answerToAdd);
                }
            }

            foreach (var answer in answersToUpdate)
            {
                await _uow.UserAnwserRepository.Update(answer);
            }

            foreach (var answer in answersToAdd)
            {
                await _uow.UserAnwserRepository.Create(answer);
            }

            await _uow.Commit();

            return existingAnswers.Concat(answersToAdd).ToList();
        }
    }
    }
