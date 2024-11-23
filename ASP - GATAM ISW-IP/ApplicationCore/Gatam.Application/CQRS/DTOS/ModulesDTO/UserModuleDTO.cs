using Gatam.Application.CQRS;
using Gatam.Application.CQRS.DTOS.ModulesDTO;
using Gatam.Application.CQRS.DTOS.QuestionsDTO;
using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.DTOS.ModulesDTO
{
public class UserModuleDTO
{
    public string Id { get; set; }
        public string UserId { get; set; }
        public UserDTO User { get; set; }
        public ModuleDTO Module { get; set; } 
        public List<UserQuestionDTO> UserQuestion { get; set; }
        public List<UserAnswer> UserGivenAnswers { get; set; } = new List<UserAnswer>();
    }
}
