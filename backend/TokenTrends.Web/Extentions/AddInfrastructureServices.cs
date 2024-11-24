using System.Net;
using System.Text;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Minio.DataModel.Args;
using TokenTrends.Application.Abstractions.Services;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Application.Services.Email;
using TokenTrends.Application.Services.FileServices;
using TokenTrends.Infrastructure.Services;
using TokenTrends.Infrastructure.Services.Authentication;
using TokenTrends.Infrastructure.Services.Email;
using TokenTrends.Infrastructure.Services.FileService;
using TokenTrends.Infrastructure.Services.Options;

namespace TokenTrends.Web.Extentions;

public static class AddInfrastructureServices 
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddAuthenticationService(services, configuration);
        services.AddAuthorization();
        AddEmailService(services, configuration);
        AddFileService(services, configuration);
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

    private static void AddFileService(this IServiceCollection services, IConfiguration configuration)
    {
        var optionsConfiguration = configuration.GetSection("S3Options");
        services.Configure<S3Options>(optionsConfiguration);
        var options = optionsConfiguration.Get<S3Options>();
        
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(options.Endpoint)
            .WithCredentials(options.AccessKey, options.SecretKey)
            .WithSSL(false)
            .Build());

        services.AddScoped<IFileService, FileService>();
    }
    
    private static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        var emailOptionsConfiguration = configuration.GetSection("Smtp");
        services.Configure<EmailOptions>(emailOptionsConfiguration);
        
        var emailOptions = emailOptionsConfiguration.Get<EmailOptions>();
        
        if (emailOptions is null) throw new Exception("Smtp secrets are missing");
        
        services.AddTransient<SmtpClient>(s =>
        {
            var smtpClient = new SmtpClient();
            
            smtpClient.Connect(emailOptions.Host, emailOptions.Port);
            smtpClient.Authenticate(emailOptions.Username, emailOptions.Password);
            
            return smtpClient;
        });
        
        services.AddTransient<IEmailService, EmailService>();
    }
}