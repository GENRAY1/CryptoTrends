using TokenTrends.Domain.Absractions;

namespace TokenTrends.Domain.Account.Identity;

public class Role : Entity
{
    public const int MaxNameLength = 32;
    public const int MinNameLength = 2;

    public const int MaxDescriptionLength = 300;
    private Role()
    {
        
    }

    public static Role Admin { get; } = new Role
    {
        Id = new Guid("cc3b8540-aaf6-4ea6-9685-0d752231b1bf"),
        Name = "Admin",
        Description = null
    };

    public static Role User { get; } = new Role 
    { 
        Id = new Guid("8d4b5a5f-2f6a-4f6e-9a1b-5f4b5e5f6b5b"), 
        Name = "User", 
        Description = null
    };

    public static IReadOnlyCollection<Role> Roles { get; } = new[] { Admin, User };
    
    public string Name { get; private init; }
    
    public string? Description { get; private init; }
}