using System.Text.Json.Serialization;

namespace Gatam.Domain;

public class UserModule
{
    public string UserId { get; set; }
    [JsonIgnore] 
    public ApplicationUser User { get; set; }

    public string ModuleId { get; set; }
    [JsonIgnore] 
    public ApplicationModule Module { get; set; }
}