using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gatam.Domain
{
    public class QuestionAnswer
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Je moet een antwoord meegeven")]
        public string Answer {  get; set; }
        public string? AnswerValue { get; set; }
        public string QuestionId { get; set; }
        public Question? Question { get; set; }
        public List<UserAnswer>? GivenUserAnswers { get; set; } = new List<UserAnswer>();

        public QuestionAnswer() { 
            Id = Guid.NewGuid().ToString();
        }

    }
}
