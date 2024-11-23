using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;

namespace TokenTrends.Infrastructure.DataAccess.Configurations;

public class ConfirmationCodeConfiguration : IEntityTypeConfiguration<ConfirmationCode>
{
    public void Configure(EntityTypeBuilder<ConfirmationCode> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<Account>()
            .WithMany()
            .HasForeignKey(x => x.AccountId);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.ExpiredAt)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(ConfirmationCode.CodeLength);

        builder.Property(x => x.Event)
            .IsRequired();
    }
}