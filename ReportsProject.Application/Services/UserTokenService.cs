using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReportsProject.Application.Resources.ErrorMessage;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Enums;
using ReportsProject.Domain.Interfaces.Repositories;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Models;
using ReportsProject.Domain.Results;
using ReportsProject.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReportsProject.Application.Services;

public class UserTokenService : IUserTokenService
{
	private readonly IBaseRepository<User> _userRepository;


	private readonly string _jwtKey;
	private readonly string _issuer;
	private readonly string _audience;

	public UserTokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepository)
	{
		_jwtKey = options.Value.JwtKey;
		_issuer = options.Value.Issuer;
		_audience = options.Value.Audience;
		_userRepository = userRepository;
	}

	public string GenerateRefreshToken()
	{
		var randomBytes = new byte[32];
		using var bytesGenerator = RandomNumberGenerator.Create();
		bytesGenerator.GetBytes(randomBytes);

		return Convert.ToBase64String(randomBytes);
	}

	public string GenerateAccessToken(IEnumerable<Claim> claims)
	{
		var expires = DateTime.UtcNow.AddMinutes(10);

		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
		var securityToken = new JwtSecurityToken(
			_issuer, _audience, claims, null, expires, credentials);
		var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

		return token;
	}

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var tokenValdationParameters = new TokenValidationParameters()
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			ValidateLifetime = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
			ValidAudience = _audience,
			ValidIssuer = _issuer
		};
		
		var algorithm = SecurityAlgorithms.HmacSha256;
		var stringComparison = StringComparison.InvariantCultureIgnoreCase;
		var tokenHandler = new JwtSecurityTokenHandler();
		var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValdationParameters, out var securityToken);

		if (securityToken is not JwtSecurityToken jwtSecurityToken ||
			!jwtSecurityToken.Header.Alg.Equals(algorithm, stringComparison))
		{
			throw new SecurityTokenException(ErrorMessage.InvalidToken);
		}

		return claimsPrincipal;
			
			
	}

	public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto)
	{
		var accessToken = tokenDto.AccessToken;
		var refreshToken = tokenDto.RefreshToken;

		var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
		var userLogin = claimsPrincipal.Identity?.Name;

		var foundUser = await _userRepository.GetAll()
			.Include(user => user.UserToken)
			.FirstOrDefaultAsync(user => user.Login == userLogin);

		if (foundUser == null ||
			foundUser.UserToken.RefreshToken != refreshToken ||
			foundUser.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.InvalidClientRequest,
				ErrorCode = (int)ErrorCodes.InvalidClientRequest
			};
		}

		var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
		var newRefreshToken = GenerateRefreshToken();

		foundUser.UserToken.RefreshToken = newRefreshToken;
		await _userRepository.UpdateAsync(foundUser);

		return new BaseResult<TokenDto>
		{
			Data = new TokenDto
			{
				RefreshToken = newRefreshToken,
				AccessToken = newAccessToken
			}
		};
	}
}
