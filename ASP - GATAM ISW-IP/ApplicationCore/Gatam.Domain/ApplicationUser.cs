using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;


namespace Gatam.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; }
        [JsonIgnore]
        public ICollection<UserModule> UserModules { get; set; } = new List<UserModule>();

        public string? BegeleiderId { get; set; }
    }
}
