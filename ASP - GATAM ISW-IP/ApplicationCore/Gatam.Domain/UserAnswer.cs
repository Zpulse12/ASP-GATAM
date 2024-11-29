using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class UserAnswer
    {
        public string Id { get; set; }

        public string UserModuleId { get; set; }
        [JsonIgnore]
        public UserModule? UserModule { get; set; }

        public string? QuestionAnswerId { get; set; }
        [JsonIgnore]
        public QuestionAnswer? QuestionAnswer { get; set; }

        public string? GivenAnswer { get; set; }

        public UserAnswer()
        {
            Id = Guid.NewGuid().ToString();
        }

    }
}
