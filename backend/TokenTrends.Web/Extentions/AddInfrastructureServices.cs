using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TokenTrends.Application.Abstractions.Services;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Infrastructure.Services;
using TokenTrends.Infrastructure.Services.Authentication;
using TokenTrends.Infrastructure.Services.Options;

namespace TokenTrends.Web.Extentions;

public static class AddInfrastructureServices 
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddAuthenticationService(services, configuration);
        services.AddAuthorization();
    }

    private static void AddAuthenticationService(IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptionsConfiguration = configuration.GetSection("JwtOptions");
        services.Configure<JwtOptions>(jwtOptionsConfiguration);
        var jwtOptions = jwtOptionsConfiguration.Get<JwtOptions>();
        
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordManager, PasswordManager>();
        
        services.AddScoped<IAccountContext, AccountContext>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.AccessTokenSettings.Issuer,
                    ValidAudience = jwtOptions.AccessTokenSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.AccessTokenSettings.Key)),
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });
    }
}