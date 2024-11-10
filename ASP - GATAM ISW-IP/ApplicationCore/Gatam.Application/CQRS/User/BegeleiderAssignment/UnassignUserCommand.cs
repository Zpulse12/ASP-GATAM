﻿using AutoMapper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.User.BegeleiderAssignment
{
    public class UnassignUserCommand : IRequest<ApplicationUser>
    {
        public required string VolgerId { get; set; }
    }

    public class UnassignUserCommandHandler : IRequestHandler<UnassignUserCommand, ApplicationUser>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UnassignUserCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> Handle(UnassignUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.FindById(request.VolgerId);
            user.BegeleiderId = null;
            await _uow.UserRepository.Update(user);
            await _uow.commit();
            return user;
        }
    }
}
