using ReportsProject.Domain.Results;
using System.Net;

namespace ReportsProject.API.Middlewares;

public class AppExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly Serilog.ILogger _logger;

	public AppExceptionHandlerMiddleware(RequestDelegate next, Serilog.ILogger logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private async Task HandleExceptionAsync(HttpContext context, Exception ex)
	{
		_logger.Error(ex, ex.Message);

		var responce = ex switch
		{
			UnauthorizedAccessException _ => new BaseResult()
			{
				ErrorMessage = ex.Message,
				ErrorCode = (int)HttpStatusCode.Unauthorized,
			},

			_ => new BaseResult()
			{
				ErrorMessage = "Server error",
				ErrorCode = (int)HttpStatusCode.InternalServerError
			}
		};

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)responce.ErrorCode!;
	}
}
