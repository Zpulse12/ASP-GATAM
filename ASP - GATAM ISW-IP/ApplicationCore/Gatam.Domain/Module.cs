using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Domain
{
    public class Module
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public required string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public Module()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

    }
}
