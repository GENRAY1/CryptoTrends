namespace TokenTrends.Web.Extentions;

public static class AddMediatrServices
{
    public static void AddMediatrService(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
        });
    }
}