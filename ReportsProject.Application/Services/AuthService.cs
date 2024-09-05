using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReportsProject.Application.Resources.ErrorMessage;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Dto.User;
using ReportsProject.Domain.Enums;
using ReportsProject.Domain.Interfaces.Repositories;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Models;
using ReportsProject.Domain.Results;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReportsProject.Application.Services;

public class AuthService : IAuthService
{
	private readonly IUserTokenService _userTokenService;

	private readonly IBaseRepository<User> _userRepository;
	private readonly IBaseRepository<UserToken> _userTokenRepository;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public AuthService(
		IBaseRepository<User> userRepository,
		ILogger logger, IMapper mapper,
		IBaseRepository<UserToken> userTokenRepository,
		IUserTokenService userTokenService
	)
	{
		_userRepository = userRepository;
		_logger = logger;
		_mapper = mapper;
		_userTokenRepository = userTokenRepository;
		_userTokenService = userTokenService;
	}

	public async Task<BaseResult<TokenDto>> Login(LoginUserDto loginDto)
	{
		var foundUser = await _userRepository.GetAll()
			.FirstOrDefaultAsync(user => user.Login == loginDto.Login);

		if (foundUser == null)
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.UserNotFound,
				ErrorCode = (int)ErrorCodes.UserNotFound
			};
		}

		if (IsVerifyPassword(foundUser.PasswordHash, loginDto.Password) == false)
		{
			return new BaseResult<TokenDto>
			{
				ErrorMessage = ErrorMessage.PasswordIsWrong,
				ErrorCode = (int)ErrorCodes.PasswordIsWrong
			};
		}

		var claims = new List<Claim>
		{
			new (ClaimTypes.Name, foundUser.Login),
			new (ClaimTypes.Role, "User"),
		};

		var refreshToken = _userTokenService.GenerateRefreshToken();
		var accessToken = _userTokenService.GenerateAccessToken(claims);
		var userToken = await _userTokenRepository.GetAll()
			.FirstOrDefaultAsync(token => token.UserId == foundUser.Id);

		if (userToken == null)
		{
			userToken = new()
			{
				UserId = foundUser.Id,
				RefreshToken = refreshToken,
				RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
			};

			await _userTokenRepository.CreateAsync(userToken);
		}
		else
		{
			userToken.RefreshToken = refreshToken;
			userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

			await _userTokenRepository.UpdateAsync(userToken);
		}
			
		return new BaseResult<TokenDto>
		{
			Data = new TokenDto()
			{
				RefreshToken = refreshToken,
				AccessToken = accessToken
			}
		};
	}

	public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerDto)
	{
		if (registerDto.Password != registerDto.PasswordConfirm)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
				ErrorCode = (int)ErrorCodes.PasswordNotEqualsPasswordConfirm
			};
		}

		var foundUser = await _userRepository.GetAll()
			.FirstOrDefaultAsync(user => user.Login == registerDto.Login);

		if (foundUser != null)
		{
			return new BaseResult<UserDto>
			{
				ErrorMessage = ErrorMessage.UserAlreadyExists,
				ErrorCode = (int)ErrorCodes.UserAlreadyExists
			};
		}

		var passwordHash = HashPassword(registerDto.Password);

		var newUser = new User
		{
			Login = registerDto.Login,
			PasswordHash = passwordHash
		};

		await _userRepository.CreateAsync(newUser);

		return new BaseResult<UserDto>
		{
			Data = _mapper.Map<UserDto>(newUser)
		};
	}

	private string HashPassword(string password)
	{
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
		string passwordHash = Convert.ToBase64String(bytes);

		return passwordHash;
	}

	private bool IsVerifyPassword(string passwordHash, string password)
	{
		var hash = HashPassword(password);

		return hash == passwordHash;
	}
}
