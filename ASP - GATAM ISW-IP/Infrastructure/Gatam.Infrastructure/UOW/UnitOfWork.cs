﻿using Gatam.Application.Interfaces;
using Gatam.Infrastructure.Contexts;
using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Infrastructure.Repositories;

namespace Gatam.Infrastructure.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IGenericRepository<ApplicationTeam> _teamRepository;
        private readonly IGenericRepository<TeamInvitation> _invitationTeamRepository;


        private readonly ApplicationDbContext _context;

        public UnitOfWork(
                            ApplicationDbContext context, 
                            IGenericRepository<ApplicationUser> userRepository, 
                            IGenericRepository<ApplicationTeam> teamRepository,
                            IGenericRepository<TeamInvitation> invitationTeamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _invitationTeamRepository = invitationTeamRepository;
            _context = context;
        }

        public IGenericRepository<ApplicationUser> UserRepository => _userRepository;

        public IGenericRepository<ApplicationTeam> TeamRepository => _teamRepository;

        public IGenericRepository<TeamInvitation> TeamInvitationRepository => _invitationTeamRepository;

        public async Task commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
