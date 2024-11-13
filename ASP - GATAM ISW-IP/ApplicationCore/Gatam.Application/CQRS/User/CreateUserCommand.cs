﻿using AutoMapper;
using FluentValidation;
using Gatam.Application.Extensions;
using Gatam.Application.Extensions.EnvironmentHelper;
using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System.Diagnostics;


namespace Gatam.Application.CQRS.User
{
    public class CreateUserCommand: IRequest<UserDTO>
    {
        public required UserDTO _user { get; set; }
        


    }
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManagementApi _auth0Repository;
        private readonly EnvironmentWrapper _environmentWrapper;

        public CreateUserCommandHandler(IUnitOfWork uow, IMapper mapper, IManagementApi auth0Repository, EnvironmentWrapper environmentWrapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
            _auth0Repository = auth0Repository;
            _environmentWrapper = environmentWrapper;
        }
        public async Task<UserDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {


            //request._user.Name = AESProvider.Encrypt(request._user.Name, _environmentWrapper.KEY);
            //request._user.Surname = AESProvider.Encrypt(request._user.Surname, _environmentWrapper.KEY);
            //request._user.Username = AESProvider.Encrypt(usernameForAuth0, _environmentWrapper.KEY);
            //request._user.Email = AESProvider.Encrypt(request._user.Email, _environmentWrapper.KEY);
            //request._user.PhoneNumber = AESProvider.Encrypt(request._user.PhoneNumber, _environmentWrapper.KEY);


            var createUser = await _auth0Repository.CreateUserAsync(_mapper.Map<ApplicationUser>(request._user));
           if(createUser!=null)
            {

                UserDTO user = _mapper.Map<UserDTO>(createUser);
                if (RoleMapper.Roles.TryGetValue("VOLGER", out var volgerRoleId))
                {
                    user.RolesIds = new List<string> { volgerRoleId };

                    await _auth0Repository.UpdateUserRoleAsync(user);
                }
                
                var appUser = _mapper.Map<UserDTO>(user);
                await _unitOfWork.UserRepository.Create(createUser);
                await _unitOfWork.commit();

                return appUser;
            }

            return request._user;


        }
    }
}
