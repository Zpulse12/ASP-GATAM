using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.DTOS.QuestionsDTO
{
    public class UserAnswerDTO
    {
        public string Id { get; set; }
        public string? UserModuleId { get; set; }
        public string? QuestionAnswerId { get; set; }
        public string? GivenAnswer { get; set; }
    }
}
