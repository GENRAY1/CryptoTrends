
using PetPalsProfile.Domain.Absractions;
using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Common;

namespace TokenTrends.Domain.Account.Identity;

public class RefreshToken : Entity
{
    private RefreshToken(Guid id) : base(id){}
    
    public string Value { get; private set; } = null!;
    
    public DateTime ExpirationDate { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public Guid AccountId { get; private init; } 
    
    public void Deactivate() => IsActive = false;

    public void Activate(string value, DateTime expirationDate)
    {
        IsActive = true;
        ExpirationDate = expirationDate;
        Value = value;
    }
    
    public Result Validate(DateTime utcNow, string value)  {
        
        if (IsActive == false)
            return Result.Failure(AccountErrors.RefreshTokenNotActive);

        if (ExpirationDate < utcNow)
            return Result.Failure(AccountErrors.RefreshTokenExpired);

        if (Value != value)
            return Result.Failure(AccountErrors.InvalidRefreshToken);
        
        return Result.Success();
    }

    public static RefreshToken Create(
        Guid userId, 
        string value,
        DateTime expirationDate)
    {
        return new RefreshToken(Guid.NewGuid())
        {
            Value = value,
            ExpirationDate = expirationDate,
            IsActive = true,
            AccountId = userId
        };
    }
}