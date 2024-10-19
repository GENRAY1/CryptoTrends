using Microsoft.EntityFrameworkCore;
using TokenTrends.Domain.Absractions;

namespace TokenTrends.Infrastructure.DataAccess;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}