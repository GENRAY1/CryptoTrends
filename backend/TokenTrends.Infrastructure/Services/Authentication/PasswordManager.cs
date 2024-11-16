using TokenTrends.Application.Abstractions.Services;
using TokenTrends.Application.Abstractions.Services.Authentication;

namespace TokenTrends.Infrastructure.Services.Authentication;

public class PasswordManager : IPasswordManager
{
    public string Generate(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    
    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}