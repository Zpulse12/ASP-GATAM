﻿using AutoMapper;
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

namespace Gatam.Application.CQRS.Questions
{
    public class UpdateQuestionCommand : IRequest<Question>
    {
        public string Id { get; set; }

        public required Question Question { get; set; }
    }
    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateQuestionCommand, Question>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UpdateUserCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Question> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _uow.QuestionRepository.FindById(request.Id);

            if (question == null)
            {
                throw new Exception($"Question with ID {request.Id} was not found.");
            }

            _mapper.Map(request.Question, question);
            var updatedQuestion = await _uow.QuestionRepository.Update(question);
            await _uow.Commit();
            return _mapper.Map<Question>(updatedQuestion);
        }
    }
}
