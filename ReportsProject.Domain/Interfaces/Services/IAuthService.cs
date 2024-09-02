using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Dto.User;
using ReportsProject.Domain.Results;

namespace ReportsProject.Domain.Interfaces.Services;

public interface IAuthService
{
	/// <summary>
	/// Регистрация пользователя
	/// </summary>
	/// <param name="registerUserDto"></param>
	/// <returns></returns>
	public Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto);

	/// <summary>
	/// Авторизация пользователя
	/// </summary>
	/// <param name="registerUserDto"></param>
	/// <returns></returns>
	public Task<BaseResult<TokenDto>> Login(LoginUserDto registerUserDto);
}
