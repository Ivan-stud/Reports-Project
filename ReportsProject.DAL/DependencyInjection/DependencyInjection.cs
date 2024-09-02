using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportsProject.DAL.Interceptors;
using ReportsProject.DAL.Repositories;
using ReportsProject.Domain.Interfaces.Repositories;
using ReportsProject.Domain.Models;

namespace ReportsProject.DAL.DependencyInjection;

public static class DependencyInjection
{
	public static void AddPostgreDbContext(this IServiceCollection services, IConfiguration configuration)
	{
		string connectionString = configuration.GetConnectionString("PostgreSQL") ?? throw new InvalidOperationException();

		services.AddSingleton<DateInterceptor>();

		services.AddDbContext<AppDbContext>(options =>
		{
			options.UseNpgsql(connectionString);
		});

		services.InitRepositories();
	}

	private static void InitRepositories(this IServiceCollection services)
	{
		services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
		services.AddScoped<IBaseRepository<Report>, BaseRepository<Report>>();
		services.AddScoped<IBaseRepository<UserToken>, BaseRepository<UserToken>>();
	}
}
