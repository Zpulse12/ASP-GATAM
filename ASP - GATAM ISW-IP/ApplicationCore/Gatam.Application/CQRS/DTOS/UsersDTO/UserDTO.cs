namespace Gatam.Application.CQRS
{
    public class UserDTO
    {
        public required string Nickname { get; set; }
        public required string Email { get; set; }
        public required IEnumerable<string> RolesIds { get; set; } 

        public required string Id { get; set; }
        public bool IsActive { get; set; }
        public string Picture { get; set; }
        
    }
}
