using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Models;
using ReportsProject.Domain.Results;

namespace ReportsProject.API.Controllers;

[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
	private readonly IReportService _reportService;

	public ReportController(IReportService reportService)
	{
		_reportService = reportService;
	}

	/// <summary>
	///	Получение отчета пользвателя
	/// </summary>
	/// <param name="userId"></param>
	/// <response code="200">Отчет был успешно получен</response>
	/// <remarks>
	/// GET Request
	/// </remarks>
	[HttpGet("get-all/{userId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<ReportDto>>> GetAll(int userId)
	{
		var response = await _reportService.GetAllAsync(userId);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpGet("get/{reportId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<ReportDto>>> Get(int reportId)
	{
		var response = await _reportService.GetAsync(reportId);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpDelete("delete/{reportId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<ReportDto>>> Delete(int reportId)
	{
		var response = await _reportService.RemoveAsync(reportId);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	/// <summary>
	///	Создание отчета пользвателя
	/// </summary>
	/// <param name="createDto"></param>
	/// <response code="200">Отчет был успешно создан</response>
	/// <response code="400">Ошибка при создании отчета</response>
	/// <remarks>
	/// Default request:
	/// 
	///		POST
	///		{
	///			"name": "Test Name",
	///			"description": "Test Description",
	///			"userId": "1"
	///		}
	///	
	/// </remarks>
	[HttpPost("create")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<ReportDto>>> Create([FromBody] CreateReportDto createDto)
	{
		var response = await _reportService.CreateAsync(createDto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

	[HttpPut("update")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<ReportDto>>> Update([FromBody] UpdateReportDto updateDto)
	{
		var response = await _reportService.UpdateAsync(updateDto);

		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}

}

