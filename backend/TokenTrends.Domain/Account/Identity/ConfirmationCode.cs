using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account.Identity.Enums;

namespace TokenTrends.Domain.Account.Identity;

public class ConfirmationCode : Entity
{
    public const int CodeLength = 64;
    public const int CodeLifeTimeInMinutes = 15;
    public const int MaxAttemptCount = 3;
    public ConfirmationCodeEvent Event { get; private init; }
    public ConfirmationCodeStatus Status { get; private set; }
    public string Code { get; private init; } = null!;
    public DateTime CreatedAt { get; private init; }
    public DateTime ExpiredAt { get; private init; }
    public DateTime StatusChangedAt { get; private set; }
    public Guid AccountId { get; private init; }
    
    
    public bool IsExpired => DateTime.UtcNow > ExpiredAt;
    
    public static ConfirmationCode Create(Guid accountId, ConfirmationCodeEvent codeEvent)
    {
        var dateNow = DateTime.UtcNow;
        return new ConfirmationCode
        {
            Id = Guid.NewGuid(),
            Event = codeEvent,
            Code = GenerateCode(),
            CreatedAt = dateNow,
            Status = ConfirmationCodeStatus.Pending,
            StatusChangedAt = dateNow,
            ExpiredAt = DateTime.UtcNow.AddMinutes(CodeLifeTimeInMinutes),
            AccountId = accountId
        };
    }
    
    public void Confirm()
    {
        Status = ConfirmationCodeStatus.Confirmed;
        StatusChangedAt = DateTime.UtcNow;
    }
    
    public void Fail()
    {
        Status = ConfirmationCodeStatus.Failed;
        StatusChangedAt = DateTime.UtcNow;
    }
    
    private static string GenerateCode()
    {
        char[] numberChars = [
            '0', 'd', 'c', 'b', 'a', 'n','l', 'm', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y',
            'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R',
            'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '?'
        ];

        var random = new Random();
        
        string otpCode = new string(Enumerable.Repeat(numberChars, CodeLength)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        
        return otpCode;
    }
}