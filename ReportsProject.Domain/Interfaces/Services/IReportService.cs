using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Results;

namespace ReportsProject.Domain.Interfaces.Services;

public interface IReportService
{
	/// <summary>
	/// Получение всех отчетов пользователя
	/// </summary>
	/// <param name="userId"></param>
	/// <returns></returns>
	public Task<CollectionResult<ReportDto>> GetAllAsync(int userId);

	/// <summary>
	/// Получение отчета по его Id
	/// </summary>
	/// <param name="reportId"></param>
	/// <returns></returns>
	public Task<BaseResult<ReportDto>> GetAsync(int reportId);

	/// <summary>
	/// Создание отчета
	/// </summary>
	/// <param name="createReportDto"></param>
	/// <returns></returns>
	public Task<BaseResult<ReportDto>> CreateAsync(CreateReportDto createReportDto);

	/// <summary>
	/// Удаление отчета по его Id
	/// </summary>
	/// <param name="reportId"></param>
	/// <returns></returns>
	public Task<CollectionResult<ReportDto>> RemoveAsync(int reportId);

	/// <summary>
	/// Обновить данные отчета
	/// </summary>
	/// <returns></returns>
	public Task<BaseResult<ReportDto>> UpdateAsync(UpdateReportDto updateReportDto);
}
