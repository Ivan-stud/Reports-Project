using ReportsProject.Application.Resources.ErrorMessage;
using ReportsProject.Domain.Enums;
using ReportsProject.Domain.Interfaces.Validations;
using ReportsProject.Domain.Models;
using ReportsProject.Domain.Results;

namespace ReportsProject.Application.Validations;

public class ReportValidator : IReportValidator
{
	public BaseResult Validate(Report report, User user)
	{
		if (report != null)
		{
			return new BaseResult()
			{
				ErrorMessage = ErrorMessage.ReportAlreadyExists,
				ErrorCode = (int) ErrorCodes.ReportAlreadyExists
			};
		}

		if (user == null)
		{
			return new BaseResult()
			{
				ErrorMessage = ErrorMessage.UserNotFound,
				ErrorCode = (int)ErrorCodes.UserNotFound,
			};
		}

		return new BaseResult();
	}

	public BaseResult ValidateOnNull(Report? model)
	{
		if (model == null)
		{
			return new BaseResult()
			{
				ErrorMessage = ErrorMessage.ReportNotFound,
				ErrorCode = (int) ErrorCodes.ReportNotFound
			};
		}

		return new BaseResult();
	}
}
