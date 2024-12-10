using Gatam.Shared.Attributes;

namespace Gatam.Domain
{
    public class ApplicationUser 
    {
        public string? Id { get; set; }

        [Encrypted]
        public string Name { get; set; }
        [Encrypted]
        public  string Surname { get; set; }
        [Encrypted]
        public string Username { get; set; }
        [Encrypted]
        public  string Email { get; set; }
        [Encrypted]
        public  string? PhoneNumber { get; set; }
        public  string? PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public string? Picture { get; set; }

        public ICollection<UserModule>? UserModules { get; set; } = new List<UserModule>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public string? MentorId { get; set; }
    }
}

