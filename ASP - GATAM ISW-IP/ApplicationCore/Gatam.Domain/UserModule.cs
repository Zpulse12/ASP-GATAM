using System.Text.Json.Serialization;

namespace Gatam.Domain;

public class UserModule
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public string ModuleId { get; set; }
    public ApplicationModule Module { get; set; }
}