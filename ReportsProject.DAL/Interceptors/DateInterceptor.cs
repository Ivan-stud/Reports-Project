using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReportsProject.Domain.Interfaces.Models;

namespace ReportsProject.DAL.Interceptors;

public class DateInterceptor : SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		var dbContext = eventData.Context;

		if (dbContext == null)
			return base.SavingChangesAsync(eventData, result, cancellationToken);

		ChangeEntries(dbContext);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		var dbContext = eventData.Context;

		if (dbContext == null)
			return base.SavingChanges(eventData, result);

		ChangeEntries(dbContext);

		return base.SavingChanges(eventData, result);
	}

	private void ChangeEntries(DbContext dbContext)
	{
		var entries = dbContext.ChangeTracker.Entries<IAuditable<int>>();

		foreach (var entry in entries)
		{
			DateTime now = DateTime.UtcNow;

			if (entry.State == EntityState.Added)
			{
				entry.Property(p => p.CreatedAt).CurrentValue = now;
				entry.Property(p => p.UpdatedAt).CurrentValue = now;
			}

			if (entry.State == EntityState.Modified)
			{
				entry.Property(p => p.UpdatedAt).CurrentValue = now;
			}
		}
	}
}
