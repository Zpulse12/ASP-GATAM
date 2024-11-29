namespace Gatam.Domain
{
    public class ApplicationUser 
    {
        public string? Id { get; set; }

        public string Name { get; set; }
        public  string Surname { get; set; }
        public string Username { get; set; } 
        public  string Email { get; set; }
        public  string? PhoneNumber { get; set; }
        public  string PasswordHash { get; set; }
        public  List<string?> RolesIds { get; set; }
        public bool IsActive { get; set; }
        public string? Picture { get; set; }

        public ICollection<UserModule>? UserModules { get; set; } = new List<UserModule>();

        public string? BegeleiderId { get; set; }
    }
}

