using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Interfaces.Repositories;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Models;
using ReportsProject.Domain.Results;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ReportsProject.Application.Resources.ErrorMessage;
using ReportsProject.Domain.Enums;
using ReportsProject.Domain.Interfaces.Validations;
using AutoMapper;

namespace ReportsProject.Application.Services;

public class ReportService : IReportService
{
	private readonly IBaseRepository<Report> _reportRepository;
	private readonly IBaseRepository<User> _userRpository;
	private readonly IReportValidator _reportValidator;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

	public ReportService(
		IBaseRepository<Report> reportRepository,
		ILogger logger,
		IBaseRepository<User> userRpository,
		IReportValidator reportValidator,
		IMapper mapper)
	{
		_reportRepository = reportRepository;
		_userRpository = userRpository;
		_reportValidator = reportValidator;
		_logger = logger;
		_mapper = mapper;
	}

	/// <inheritdoc/>
	public async Task<BaseResult<ReportDto>> CreateAsync(CreateReportDto createReportDto)
	{
		try
		{
			var userId = createReportDto.UserId;
			var reportName = createReportDto.Name;
			var reportDescription = createReportDto.Description;
			var foundUser = await _userRpository.GetAll().FirstOrDefaultAsync(user => user.Id == userId);
			var foundReport = await _reportRepository.GetAll()
				.FirstOrDefaultAsync(report => report.UserId == userId && report.Name == reportName);

			BaseResult validatedResult = _reportValidator.Validate(foundReport, foundUser);

			if (validatedResult.IsSuccess == false)
			{
				return new BaseResult<ReportDto>()
				{
					ErrorMessage = validatedResult.ErrorMessage,
					ErrorCode = validatedResult.ErrorCode
				};
			}

			var report = new Report()
			{
				Name = reportName,
				Description = reportDescription,
				UserId = userId,
			};

			await _reportRepository.CreateAsync(report);

			var reportDto = _mapper.Map<ReportDto>(report);
			var result = new BaseResult<ReportDto>(){ Data = reportDto };

			return result;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return GetResultOnException();
		}
	}

	/// <inheritdoc/>
	public Task<BaseResult<ReportDto>> GetAsync(int reportId)
	{
		ReportDto? reportDto;

		try
		{
			reportDto = _reportRepository.GetAll()
				.AsEnumerable()
				.Select(report => new ReportDto(report.Id, report.Name, report.Description, report.CreatedAt.ToLongDateString()))
				.FirstOrDefault(report => report.Id == reportId);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return Task.FromResult(GetResultOnException());
		}

		if (reportDto == null)
		{
			var warningMessage = ErrorMessage.ReportNotFound;
			int errorCode = (int)ErrorCodes.ReportNotFound;

			_logger.Warning($"{warningMessage}: {reportId}");

			var warningResult = new BaseResult<ReportDto>()
			{
				ErrorCode = errorCode,
				ErrorMessage = warningMessage,
			};

			return Task.FromResult(warningResult);
		}

		var result = new BaseResult<ReportDto>(){ Data = reportDto };

		return Task.FromResult(result);
	}

	/// <inheritdoc/>
	public Task<CollectionResult<ReportDto>> GetAllAsync(int userId)
	{
		ReportDto[] reports;

		try
		{
			reports = _reportRepository.GetAll()
				.Where(report => report.UserId == userId)
				.AsEnumerable()
				.Select(report => new ReportDto(report.Id, report.Name, report.Description, report.CreatedAt.ToLongDateString()))
				.ToArray();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return Task.FromResult(GetCollectionResultOnException());
		}

		if (reports.Length == 0)
		{
			var warningMessage = ErrorMessage.ReportsNotFound;
			int errorCode = (int) ErrorCodes.ReportsNotFound;

			_logger.Warning(warningMessage, reports.Length);

			var emptyResult = new CollectionResult<ReportDto>
			{
				ErrorCode = errorCode,
				ErrorMessage = warningMessage
			};

			return Task.FromResult(emptyResult);
		}

		var result = new CollectionResult<ReportDto>()
		{
			Data = reports,
			Count = reports.Length
		};

		return Task.FromResult(result);
	}

	/// <inheritdoc/>
	public async Task<CollectionResult<ReportDto>> RemoveAsync(int reportId)
	{
		try
		{
			var foundedReport = _reportRepository.GetAll().FirstOrDefault(report => report.Id == reportId);
			var validatedReport = _reportValidator.ValidateOnNull(foundedReport);

			if (validatedReport.IsSuccess == false)
			{
				return new CollectionResult<ReportDto>
				{
					ErrorMessage = validatedReport.ErrorMessage,
					ErrorCode = validatedReport.ErrorCode
				};
			}

			int userId = foundedReport!.UserId;
			await _reportRepository.RemoveAsync(foundedReport!);
			var result = GetAllAsync(userId);

			return await result;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return GetCollectionResultOnException();
		}
	}

	public async Task<BaseResult<ReportDto>> UpdateAsync(UpdateReportDto updateReportDto)
	{
		try
		{
			var foundedReport = await _reportRepository.GetAll()
				.FirstOrDefaultAsync(report => report.Id == updateReportDto.Id);

			var validatedReport = _reportValidator.ValidateOnNull(foundedReport);

			if (validatedReport.IsSuccess == false)
			{
				return new BaseResult<ReportDto>
				{
					ErrorMessage = validatedReport.ErrorMessage,
					ErrorCode = validatedReport.ErrorCode
				};
			}

			foundedReport!.Name = updateReportDto.Name;
			foundedReport!.Description = updateReportDto.Description;

			await _reportRepository.UpdateAsync(foundedReport);

			var reportDto = _mapper.Map<ReportDto>(foundedReport);
			var result = new BaseResult<ReportDto> { Data = reportDto };

			return result;
		}
		catch (Exception ex)
		{
			_logger.Error(ex, ex.Message);

			return GetResultOnException();
		}
	}

	private static BaseResult<ReportDto> GetResultOnException()
	{
		var errorResult = new BaseResult<ReportDto>()
		{
			ErrorMessage = ErrorMessage.InternalServerError,
			ErrorCode = (int)ErrorCodes.InternalServerError
		};

		return errorResult;
	}

	private static CollectionResult<ReportDto> GetCollectionResultOnException()
	{
		var errorResult = new CollectionResult<ReportDto>()
		{
			ErrorMessage = ErrorMessage.InternalServerError,
			ErrorCode = (int)ErrorCodes.InternalServerError
		};

		return errorResult;
	}
}
