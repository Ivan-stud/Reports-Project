using System.Security.Cryptography.X509Certificates;

namespace ReportsProject.Domain.Settings;

public class JwtSettings
{
	public const string DefaultSection = "Jwt";

	public string Audience { get; set; }
	public string Authority { get; set; }
	public string Issuer { get; set; }
	public string JwtKey { get; set; }
	public int Lifetime { get; set; }
	public int RefreshTokenValidityInDays { get; set; }
}
