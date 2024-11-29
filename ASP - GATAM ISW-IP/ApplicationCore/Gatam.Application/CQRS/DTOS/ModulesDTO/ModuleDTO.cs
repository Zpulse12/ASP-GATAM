using Gatam.Domain;

namespace Gatam.Application.CQRS.DTOS.ModulesDTO;

public class ModuleDTO
{
    public string Id { get; set; }
    public  string Title { get; set; }
    public  string Category { get; set; }

    public ICollection<Question> Questions { get; set; } = new List<Question>();

    public ModuleDTO()
    {
        Id = Guid.NewGuid().ToString();
    }
}