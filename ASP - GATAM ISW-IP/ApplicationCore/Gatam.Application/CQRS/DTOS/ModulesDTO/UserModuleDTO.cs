using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.DTOS.ModulesDTO
{
    public class UserModuleDTO
    {
        public string? Id { get; set; }
        public string UserId { get; set; }
        public ApplicationModule Module { get; set; }
    }
}
