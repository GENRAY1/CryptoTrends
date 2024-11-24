namespace TokenTrends.Application.Abstractions.Services.Authentication;

public interface IPasswordManager
{
    string Generate(string password);
    
    bool Verify(string password, string hash);
}