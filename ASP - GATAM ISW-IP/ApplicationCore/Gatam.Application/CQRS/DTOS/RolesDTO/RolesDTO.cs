using System.Text.Json.Serialization;

namespace Gatam.Application.CQRS.DTOS.RolesDTO
{
    public class RolesDTO
    {
        [JsonPropertyName("roles")]
        public List<string?> Roles { get; set; }
    }
}
