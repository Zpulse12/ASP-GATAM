namespace Gatam.Application.CQRS.DTOS.ModulesDTO;
public class UserWithModulesDTO
{
    public string Id { get; set; }
    public string Email { get; set; }
    public ICollection<string> Modules { get; set; }
}
