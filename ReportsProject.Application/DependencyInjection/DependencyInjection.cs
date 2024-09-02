using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ReportsProject.Application.Mappings;
using ReportsProject.Application.Services;
using ReportsProject.Application.Validations;
using ReportsProject.Application.Validations.FluentValidation;
using ReportsProject.Domain.Dto;
using ReportsProject.Domain.Interfaces.Services;
using ReportsProject.Domain.Interfaces.Validations;

namespace ReportsProject.Application.DependencyInjection;

public static class DependencyInjection
{
	public static void AddApplication(this IServiceCollection services)
	{
		services.AddAutoMapper(typeof(ReportMapping));

		InitServices(services);
	}

	private static void InitServices(this IServiceCollection services)
	{
		services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
		services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();
		
		services.AddScoped<IReportValidator, ReportValidator>();

		services.AddScoped<IReportService, ReportService>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IUserTokenService, UserTokenService>();
		
	}
}
