namespace Gatam.Application.CQRS
{
    public class UserDTO
    {
        public required string Nickname { get; set; }
        public required string Email { get; set; }
        public required IEnumerable<string> RolesIds { get; set; } 
        public string? BegeleiderId { get; set; }
        public required string Id { get; set; }
        public bool IsActive { get; set; }
        public string Picture { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Modules { get; set; }

        
    }
}
