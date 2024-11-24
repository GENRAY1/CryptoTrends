using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Services.Email;

public interface IEmailService
{
    Task SendEmailAsync(string email, string title, string body);
}