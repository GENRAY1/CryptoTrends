using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TokenTrends.Domain.Account.Identity;

namespace TokenTrends.Infrastructure.DataAccess.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .HasMaxLength(Role.MaxNameLength);
        
        builder.Property(r => r.Description)
            .HasMaxLength(Role.MaxDescriptionLength);
        
        builder.HasData(Role.Roles);
    }
}