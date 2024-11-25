using Gatam.Application.Interfaces;
using Gatam.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.Module.UserModules
{
    public class UpdateUserModuleStatusCommand : IRequest<UserModule>
    {
        public string UserModuleId { get; set; }
        public UserModuleState State { get; set; }
    }
    public class UpdateUserModuleStatusHandler : IRequestHandler<UpdateUserModuleStatusCommand, UserModule>
    {
        private readonly IUserModuleRepository _userModuleRepository;

        public UpdateUserModuleStatusHandler(IUserModuleRepository userModuleRepository)
        {
            _userModuleRepository = userModuleRepository;
        }

        public async Task<UserModule> Handle(UpdateUserModuleStatusCommand request, CancellationToken cancellationToken)
        {
            var userModule = await _userModuleRepository.FindByIdModuleWithIncludes(request.UserModuleId);
            userModule.State = request.State;
            await _userModuleRepository.Update(userModule);
            return userModule;
        }
    }
}

