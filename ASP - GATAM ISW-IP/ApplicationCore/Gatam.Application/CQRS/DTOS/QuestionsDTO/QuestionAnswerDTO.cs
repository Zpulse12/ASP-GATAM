using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.DTOS.QuestionsDTO
{
    public class QuestionAnswerDTO
    {
        public string Id { get; set; }
        public string Answer { get; set; }
        public string? AnswerValue { get; set; }
        public string QuestionId { get; set; }
    }

}
