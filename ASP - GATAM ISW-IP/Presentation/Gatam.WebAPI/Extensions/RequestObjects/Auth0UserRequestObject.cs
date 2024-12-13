using System.Text.Json.Serialization;

namespace Gatam.WebAPI.Extensions.RequestObjects
{
    public class Auth0UserRequestObject
    {
        // (Nils) Json properties zijn nodig om juist te kunnen mappen. Deze klassen komt overeen met de data die we post registratie via auth0 doorsturen.
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        [JsonPropertyName("profile_picture")]
        public string ProfilePicture { get; set; }
        public override string ToString()
        {
            return $"UserData: {{ Email: {Email}, Username: {Username}, Name: {Name}, UserId: {UserId} }}";
        }
    }

}
