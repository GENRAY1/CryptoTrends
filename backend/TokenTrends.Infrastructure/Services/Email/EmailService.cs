
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PetPalsProfile.Domain.Absractions;
using TokenTrends.Application.Abstractions.Services.Email;
using TokenTrends.Domain.Common;
using TokenTrends.Infrastructure.Services.Options;

namespace TokenTrends.Infrastructure.Services.Email;

public class EmailService(
    IOptions<EmailOptions> options,
    SmtpClient smtpClient
    ) : IEmailService
{
    public async Task<Result> SendEmailAsync(string email, string title, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.Value.FromName, options.Value.From));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = title;
        
        message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = body
        };

        try
        {
           await smtpClient.SendAsync(message);
        }
        catch (Exception e)
        {
            return Result.Failure(new Error("EmailService.SendEmailFailed", e.Message));
        }
        
        return Result.Success();
    }
}