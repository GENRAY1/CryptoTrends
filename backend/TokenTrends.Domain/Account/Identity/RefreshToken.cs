using TokenTrends.Domain.Absractions;

namespace TokenTrends.Domain.Account.Identity;

public class RefreshToken : Entity
{
    private RefreshToken(Guid id) : base(id){}
    
    public string Value { get; private set; } = null!;
    
    public DateTime ExpirationDate { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public Guid AccountId { get; private init; } 
    
    public static RefreshToken Create(
        Guid userId, 
        string value,
        int lifeTimeInMinutes)
    {
        return new RefreshToken(Guid.NewGuid())
        {
            Value = value,
            ExpirationDate = DateTime.UtcNow.AddMinutes(lifeTimeInMinutes),
            IsActive = true,
            AccountId = userId
        };
    }
}