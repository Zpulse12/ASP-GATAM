using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class QuestionAnswer
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Je moet een antwoord meegeven")]
        public string Answer {  get; set; }
        // (Nils) value wordt gebruikt voor keuzelijst en meerkeuze vragen.
        public string? AnswerValue { get; set; }
        public string QuestionId { get; set; }
        [JsonIgnore]
        public Question? Question { get; set; }

        public List<UserAnswer> GivenUserAnswers { get; set; } = new List<UserAnswer>();

        public QuestionAnswer() { 
            Id = Guid.NewGuid().ToString();
        }

    }
}
