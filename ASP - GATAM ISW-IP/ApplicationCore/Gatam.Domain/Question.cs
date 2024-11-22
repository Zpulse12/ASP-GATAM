using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gatam.Domain
{
    public enum QuestionType
    {
        MULTIPLE_CHOICE,
        OPEN,
        TRUE_OR_FALSE,
        CHOICE_LIST
    }
    public class Question
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Je moet een vraag type meegeven")]
        public short QuestionType { get; set; }

        [Required(ErrorMessage = "Je moet een vraag meegeven")]
        [MaxLength(512, ErrorMessage = "Je vraag is te lang"), MinLength(15, ErrorMessage = "Je vraag is te kort")]
        public string QuestionTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedUserId { get; set; }
        public string? ApplicationModuleId { get; set; }
        [JsonIgnore]
        public ApplicationModule? ApplicationModule { get; set; }
        public IEnumerable<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();
        public ICollection<UserQuestion> UserQuestions { get; set; } = new List<UserQuestion>();
        public Question()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
