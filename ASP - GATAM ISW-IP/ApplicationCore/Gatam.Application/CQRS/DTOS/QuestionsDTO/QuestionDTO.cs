namespace Gatam.Application.CQRS.DTOS.QuestionsDTO;

public class QuestionDTO
{
    public string Id { get; set; }
    public int QuestionType { get; set; }
    public string QuestionTitle { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedUserId { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public string LastUpdatedUserId { get; set; }
    public string ApplicationModuleId { get; set; }
    public QuestionSettingDTO QuestionSetting { get; set; }
}