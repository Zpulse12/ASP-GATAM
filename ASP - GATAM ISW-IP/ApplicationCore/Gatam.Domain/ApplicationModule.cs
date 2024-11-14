using System.Text.Json.Serialization;

namespace Gatam.Domain
{
    public class ApplicationModule
    {
        public string Id { get; set; }
        public  string Title { get; set; }
        public  string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Question>? Questions { get; set; } = new List<Question>();
        [JsonIgnore]
        public ICollection<UserModule> UserModules { get; set; } = new List<UserModule>();

        public ApplicationModule()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }

    }
}
