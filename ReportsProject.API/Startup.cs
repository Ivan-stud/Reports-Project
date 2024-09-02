using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReportsProject.Domain.Settings;
using System.Reflection;
using System.Text;

namespace ReportsProject.API;

public static class Startup
{
	public static void AddAuthenticationAndAuthorization(this IServiceCollection services, WebApplicationBuilder builder)
	{
		services.AddAuthentication();

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(options =>
		{
			var jwtSettings = builder.Configuration.GetSection(JwtSettings.DefaultSection).Get<JwtSettings>();
			var jwtKKey = jwtSettings?.JwtKey;
			var issuer = jwtSettings?.Issuer;
			var audience = jwtSettings?.Audience;
			var authority = jwtSettings?.Authority;

			options.Authority = authority;
			options.RequireHttpsMetadata = false;
			options.TokenValidationParameters = new TokenValidationParameters()
			{
				ValidIssuer = issuer,
				ValidAudience = audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKKey!)),
				ValidateAudience = true,
				ValidateIssuer = true,
				ValidateIssuerSigningKey = true,
				ValidateLifetime = true
			};

		});
	}

	public static void AddSwager(this IServiceCollection services)
	{
		services.AddApiVersioning().AddApiExplorer(options =>
		{
			options.DefaultApiVersion = new ApiVersion(1, 0);
			options.GroupNameFormat = "'v'VVV";
			options.SubstituteApiVersionInUrl = true;
			options.AssumeDefaultVersionWhenUnspecified = true;
		});

		services.AddEndpointsApiExplorer();

		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo()
			{
				Version = "v1",
				Title = "ReportProject.API",
				Description = "This is version 1.0",
				TermsOfService = new Uri("https://github.com/vanoSTUD/Rocket-Ball"),
				Contact = new OpenApiContact()
				{
					Url = new Uri("https://github.com/vanoSTUD/Rocket-Ball")
				}
			});

			options.SwaggerDoc("v2", new OpenApiInfo()
			{
				Version = "v2",
				Title = "ReportProject.API",
				Description = "This is version 2.0",
				TermsOfService = new Uri("https://github.com/vanoSTUD/Rocket-Ball"),
				Contact = new OpenApiContact()
				{
					Url = new Uri("https://github.com/vanoSTUD/Rocket-Ball")
				}
			});

			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
			{
				In = ParameterLocation.Cookie,
				Description = "Please enter the valid token",
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				BearerFormat = "JWT",
				Scheme = "Bearer"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
			{
				{
					new OpenApiSecurityScheme()
					{
						Reference = new OpenApiReference()
						{
							Type =  ReferenceType.SecurityScheme,
							Id = "Bearer"
						},
						Name = "Bearer",
						In = ParameterLocation.Header

					},
					Array.Empty<string>()
				}
			});

			var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
		});

		
	}
}
