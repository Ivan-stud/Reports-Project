using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsProject.Domain.Models;

namespace ReportsProject.DAL.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(user => user.Id);
		builder.Property(user => user.Id).ValueGeneratedOnAdd();

		builder.Property(user => user.Login).IsRequired().HasMaxLength(20);
		builder.Property(user => user.PasswordHash).IsRequired().HasMaxLength(125);

		builder.HasMany(user => user.Reports)
			.WithOne(report => report.User)
			.HasForeignKey(report => report.UserId);

		builder.HasData(new User()
		{
			Id = 1,
			Login = "First User",
			PasswordHash = "AsdASDAsdsADaD",
			CreatedAt = DateTime.UtcNow,
			TokenId = 1
		});
	}
}
