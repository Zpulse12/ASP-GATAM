using System.Text.Json.Serialization;

namespace Gatam.Domain;


public enum QuestionPriority
{
    HIGH,
    MEDIUM,
    LOW
}

public class UserQuestion
{
    public string Id { get; set; }

    public string UserModuleId { get; set; }
    [JsonIgnore]
    public UserModule? UserModule { get; set; }

    public string QuestionId { get; set; }
    [JsonIgnore]
    public Question? Question { get; set; }
    public bool IsVisible { get; set; }
    public QuestionPriority QuestionPriority { get; set; }
    public UserQuestion()
    {
        Id = Guid.NewGuid().ToString();
        IsVisible = true;
    }
}
