using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;

namespace TokenTrends.Infrastructure.DataAccess.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(u=> u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(Account.MaxEmailLength);
        
        builder.Property(u => u.Password)
            .IsRequired()
            .HasMaxLength(Account.MaxPasswordLength);

        builder.Property(u => u.Username)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt);

        builder.Property(u => u.LastLogin);
        
        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity<AccountRole>();
    }
}