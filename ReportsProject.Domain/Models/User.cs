using ReportsProject.Domain.Interfaces.Models;

namespace ReportsProject.Domain.Models;

public class User : IAuditable<int>, IEntityId<int>
{
	public int Id { get; set; }
	public string Login { get; set; }
	public string PasswordHash{ get; set; }
	
	public List<Report> Reports { get; set; }
	public UserToken UserToken { get; set; }
	public int TokenId { get; set; }

	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
	public int CreatedBy { get; set; }
	public int? UpdatedBy { get; set; }
}
