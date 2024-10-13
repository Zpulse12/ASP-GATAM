using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gatam.Application.CQRS;
using AutoMapper;
using Gatam.Domain;
namespace Gatam.Application
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, TeamInvitationDTO>();

        }
    }
}
