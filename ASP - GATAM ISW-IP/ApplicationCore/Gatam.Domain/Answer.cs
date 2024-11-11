using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class Answer
    {
        public string Id { get; set; }
        public string AnswerTitle {  get; set; }
        public string AnswerValue { get; set; }
        public string QuestionId { get; set; }
        public Question Question { get; set; }
        public Answer() { 
            Id = Guid.NewGuid().ToString();
        }
    }
}
