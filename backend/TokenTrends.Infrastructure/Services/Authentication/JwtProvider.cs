using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Infrastructure.Services.Options;

namespace TokenTrends.Infrastructure.Services.Authentication;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly SymmetricSecurityKey _key = 
        new (Encoding.UTF8.GetBytes(options.Value.AccessTokenSettings.Key));
    
    public string GenerateAccessToken(AccountDto account)
    {
        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, account.Id.ToString()),
            new (ClaimTypes.Email, account.Email),
        };
        
        foreach (var role in account.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var token = new JwtSecurityToken(
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256),
            expires: DateTime.UtcNow.AddMinutes(options.Value.AccessTokenSettings.LifeTimeInMinutes),
            audience: options.Value.AccessTokenSettings.Audience,
            issuer: options.Value.AccessTokenSettings.Issuer,
            claims: claims
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var size = options.Value.RefreshTokenSettings.Length;
        var buffer = new byte[size];
        RandomNumberGenerator.Fill(buffer);
        return Convert.ToBase64String(buffer);
    }

    public int GetRefreshTokenLifetimeInMinutes()
    {
        return options.Value.RefreshTokenSettings.LifeTimeInMinutes;
    }

    public Guid? GetAccountIdByToken(string token)
    {
        try
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = options.Value.AccessTokenSettings.Issuer,
                ValidAudience = options.Value.AccessTokenSettings.Audience,
                IssuerSigningKey = _key,
                ClockSkew = TimeSpan.FromMinutes(0)
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var claims = jwtHandler.ValidateToken(token, tokenValidationParameters, out _);
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            return userId;
        }
        catch 
        {
            return null;
        }
    }
}