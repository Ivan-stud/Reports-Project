using ReportsProject.DAL.DependencyInjection;
using ReportsProject.Application.DependencyInjection;
using Serilog;
using ReportsProject.Domain.Settings;

namespace ReportsProject.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));

			builder.Services.AddControllers();

			builder.Services.AddSwager();

			builder.Host.UseSerilog((context, configuration) =>
			{
				configuration.ReadFrom.Configuration(context.Configuration);
			});

			builder.Services.AddPostgreDbContext(builder.Configuration);

			builder.Services.AddApplication();

			builder.Services.AddAuthenticationAndAuthorization(builder);

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportsProject Swagger v 1.0");
					options.SwaggerEndpoint("/swagger/v2/swagger.json", "ReportsProject Swagger v 2.0");
					//options.RoutePrefix = "";
				});
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
