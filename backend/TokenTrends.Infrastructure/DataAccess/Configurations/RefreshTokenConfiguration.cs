using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;

namespace TokenTrends.Infrastructure.DataAccess.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Value)
            .IsRequired();
        
        builder.Property(r => r.ExpirationDate)
            .IsRequired();
        
        builder.Property(r => r.IsActive)
            .IsRequired();
        
        builder.Property(r => r.AccountId)
            .IsRequired();

        builder.HasOne<Account>()
            .WithOne()
            .HasForeignKey<RefreshToken>(r => r.AccountId);
    }
}