using Microsoft.EntityFrameworkCore;
using TokenTrends.Domain.Account;
using TokenTrends.Domain.Account.Identity;
using TokenTrends.Infrastructure.DataAccess;
using TokenTrends.Infrastructure.DataAccess.Repositories;

namespace TokenTrends.Web.Extentions;

public static class AddStorageServices
{
    public static void AddStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string not found");
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }
}