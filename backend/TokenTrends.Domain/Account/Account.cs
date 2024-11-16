using TokenTrends.Domain.Absractions;
using TokenTrends.Domain.Account.Identity;

namespace TokenTrends.Domain.Account;

public class Account : Entity
{
    public const int MaxEmailLength = 256;
    public const int MaxPasswordLength = 64;
    public const int MinPasswordLength = 6;
    
    private List<Role> _roles = new();
    
    public string Email { get; private set; } = null!;

    public string Password { get; private set; } = null!;

    public string Username { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }
    
    public DateTime? LastLogin { get; private set; }
    public IReadOnlyCollection<Role> Roles => _roles;
    
    public void AddRole(Role role)
    {
        if (!_roles.Contains(role))
        {
            _roles.Add(role);
        }

        if (LastLogin is not null)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void RemoveRole(Role role)
    {
        bool isRemoved = _roles.Remove(role);
        
        if (isRemoved)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Logged () => LastLogin = DateTime.UtcNow;
    
    public static Account Create(
        string email, 
        string password,
        string username)
    {
        var user = new Account
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = password,
            Username = username,
            CreatedAt = DateTime.UtcNow,
        };
        
        user.AddRole(Role.User);
        
        return user;
    }
}