using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gatam.Domain;

public class UserModule
{
    public string Id { get; set; }
    public string UserId { get; set; }
    [JsonIgnore] 
    public ApplicationUser? User { get; set; }

    public string ModuleId { get; set; }
    [JsonIgnore] 
    public ApplicationModule? Module { get; set; }

    public UserModuleState State { get; set; }

    public ICollection<UserAnswer> UserGivenAnswers { get; set; } = new List<UserAnswer>();
    public List<UserQuestion> UserQuestions { get; set; } = new List<UserQuestion>();

    public UserModule()
    {
        Id = Guid.NewGuid().ToString();
        State = UserModuleState.NotStarted;
    }
}

public enum UserModuleState
{
    NotStarted, InProgress
}