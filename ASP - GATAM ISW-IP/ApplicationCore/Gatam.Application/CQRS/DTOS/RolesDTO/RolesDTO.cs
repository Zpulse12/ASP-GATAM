using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.DTOS.RolesDTO
{
    public class RolesDTO
    {
        [JsonPropertyName("roles")]
        public List<string?>? Roles { get; set; }
    }
}
