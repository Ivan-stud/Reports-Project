using Microsoft.AspNetCore.Mvc;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Dto.User;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Results;

namespace ReportsProject.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		/// <summary>
		/// Регистрация пользователя
		/// </summary>
		/// <param name="registerUserDto"></param>
		/// <returns></returns>
		[HttpPost("register")]
		public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto registerUserDto)
		{
			var response = await _authService.Register(registerUserDto);

			if (response.IsSuccess)
				return Ok(response);

			return BadRequest(response);
		}

		/// <summary>
		/// Авторизация пользователя
		/// </summary>
		/// <param name="loginUserDto"></param>
		/// <returns></returns>
		[HttpPost("login")]
		public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto loginUserDto)
		{
			var response = await _authService.Login(loginUserDto);

			if (response.IsSuccess)
				return Ok(response);

			return BadRequest(response);
		}
	}
}
