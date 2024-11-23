using TokenTrends.Domain.Common;

namespace TokenTrends.Application.Abstractions.Services.Email;

public interface IEmailService
{
    Task<Result> SendEmailAsync(string email, string title, string body);
}