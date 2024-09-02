using ReportsProject.Domain.Models;
using ReportsProject.Domain.Results;

namespace ReportsProject.Domain.Interfaces.Validations;

public interface IReportValidator : IBaseValidator<Report>
{
	/// <summary>
	/// Проверка наличия дубликата отчета, если имеется
	/// </summary>
	/// <param name="report"></param>
	/// <param name="user"></param>
	/// <returns></returns>
	public BaseResult Validate(Report report, User user);
}
