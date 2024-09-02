using ReportsProject.Domain.Interfaces.Models;

namespace ReportsProject.Domain.Models;

public class Report : IAuditable<int>, IEntityId<int>
{
	public int Id { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
	public User User { get; set; }
	public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public int CreatedBy { get; set; }
	public int? UpdatedBy { get; set; }
}
	