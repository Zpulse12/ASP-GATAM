using System.Text.Json.Serialization;

namespace Gatam.Domain;

public class UserModuleQuestionSetting
{
    public string Id { get; set; }

    public string UserModuleId { get; set; }
    public virtual UserModule UserModule { get; set; }

    public string QuestionId { get; set; }
    public virtual Question Question { get; set; }
    public bool IsVisible { get; set; }

    public UserModuleQuestionSetting()
    {
        Id = Guid.NewGuid().ToString();
        IsVisible = true;
    }
}
