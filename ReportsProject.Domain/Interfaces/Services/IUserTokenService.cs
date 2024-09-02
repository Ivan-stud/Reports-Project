using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Results;
using System.Security.Claims;

namespace ReportsProject.Domain.Interfaces.Services;

public interface IUserTokenService
{
	public string GenerateAccessToken(IEnumerable<Claim> claims);
	public string GenerateRefreshToken();
	public Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto);
	public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
}
