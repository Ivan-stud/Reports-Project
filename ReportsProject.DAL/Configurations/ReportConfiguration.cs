using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsProject.Domain.Models;

namespace ReportsProject.DAL.Configurations;

internal class ReportConfiguration : IEntityTypeConfiguration<Report>
{
	public void Configure(EntityTypeBuilder<Report> builder)
	{
		builder.HasKey(report => report.Id);
		builder.Property(report => report.Id).ValueGeneratedOnAdd();

		builder.Property(report => report.Name).IsRequired().HasMaxLength(100);
		builder.Property(report => report.Description).IsRequired().HasMaxLength(2000);

		builder.HasOne(report => report.User)
			.WithMany(user => user.Reports)
			.HasForeignKey(report => report.UserId)
			.HasPrincipalKey(user => user.Id);
	}
}