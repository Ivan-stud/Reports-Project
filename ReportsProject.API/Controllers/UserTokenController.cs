using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using ReportsProject.Application.Services;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Dto.User;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Results;

namespace ReportsProject.API.Controllers
{
	[ApiController]
	public class UserTokenController : ControllerBase
	{
		private readonly IUserTokenService _userTokenService;

		public UserTokenController(IUserTokenService userTokenService)
		{
			_userTokenService = userTokenService;
		}

		[HttpPost("refresh")]
		public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
		{
			var response = await _userTokenService.RefreshToken(tokenDto);

			if (response.IsSuccess)
				return Ok(response);

			return BadRequest(response);
		}
	}
}
