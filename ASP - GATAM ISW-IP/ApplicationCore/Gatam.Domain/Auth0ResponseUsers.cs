using System.Text.Json.Serialization;

namespace Gatam.Domain
{
    public class Auth0ResponseUsers
    {
        public DateTime CreatedAt { get; set; }
        public string Email { get; set; }
        
       
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Picture { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("user_metadata")]
        public UserMetadata UserMetadata { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
    }

    public class UserMetadata
    {
        
        public string Picture { get; set; }

        public IEnumerable<string?>? RolesIds { get; set; }
    }
}
