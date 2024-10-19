using Microsoft.EntityFrameworkCore;
using TokenTrends.Infrastructure.DataAccess;

namespace TokenTrends.Web.Extentions;

public static class AddStorageServices
{
    public static void AddStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string not found");
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    }
}