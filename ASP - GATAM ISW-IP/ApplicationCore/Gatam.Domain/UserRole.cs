namespace Gatam.Domain
{
    public class UserRole
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public ApplicationUser User { get; set; }
        public ApplicationRole Role { get; set; }
        public UserRole(){
            Id = Guid.NewGuid().ToString();
        }
    }
}