using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class Question
    {
        public string Id { get; set; }
        public required string Category { get; set; }

        public required string ModuleId { get; set; }
        [JsonIgnore]
        public ApplicationModule Module { get; set; }

        public QuestionType Type { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; } =  new List<string>();
        public bool AllowsMultipleAnswers { get; set; }
        public List<string> CorrectAnswers { get; set; } = new List<string>();

        public Question()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public enum QuestionType
    {
        TrueFalse,       
        MultipleChoice,  
        OpenText,        
        DropdownList     
    }
}
