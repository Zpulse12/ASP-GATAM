using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public QuestionType QuestionType { get; set; }
        public string QuestionTitle { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedUserId { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public string LastUpdatedUserId { get; set; }
        public ICollection<ApplicationModule> ApplicationModules { get; set; } = new List<ApplicationModule>();
        public string QuestionAnswer { get; set; }
        public Question()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }
    }
}
