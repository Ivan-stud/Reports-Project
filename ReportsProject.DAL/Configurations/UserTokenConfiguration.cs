using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReportsProject.Domain.Models;

namespace ReportsProject.DAL.Configurations;

public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
	public void Configure(EntityTypeBuilder<UserToken> builder)
	{
		builder.HasKey(token => token.Id);

		builder.Property(token => token.Id).ValueGeneratedOnAdd();
		builder.Property(token => token.RefreshToken).IsRequired();
		builder.Property(token => token.RefreshTokenExpiryTime).IsRequired();

		builder.HasData(new UserToken()
		{
			Id = 1,
			UserId = 1,
			RefreshToken = "aSDDADasADDSAdasaDSdas",
			RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
		});
	}
}
