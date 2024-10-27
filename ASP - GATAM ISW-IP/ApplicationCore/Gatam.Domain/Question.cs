using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class Question
    {
        public string Id { get; set; }
        public required string Category { get; set; }

        public required string ModuleId { get; set; }
        public Module Module { get; set; }

        public QuestionType Type { get; set; }
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public bool AllowsMultipleAnswers { get; set; }
        public List<string> CorrectAnswers { get; set; }

        public Question()
        {
            Id = Guid.NewGuid().ToString();
            Options = new List<string>();
            CorrectAnswers = new List<string>();
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
