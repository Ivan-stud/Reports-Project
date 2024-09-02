using Microsoft.EntityFrameworkCore;
using ReportsProject.DAL.Interceptors;
using System.Reflection;

namespace ReportsProject.DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
		//Database.EnsureDeleted();
		Database.EnsureCreated();
    }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(new DateInterceptor());
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
