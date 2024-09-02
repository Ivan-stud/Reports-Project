using ReportsProject.Domain.Interfaces.Models;

namespace ReportsProject.Domain.Models;

public class UserToken : IEntityId<int>
{
	public int Id { get; set; }
	public string RefreshToken { get; set; }
	public DateTime RefreshTokenExpiryTime {  get; set; }

	public User User { get; set; }
	public int UserId { get; set; }
}
