namespace TokenTrends.Domain.Account;

public interface IAccountRepository
{
    Task <Account?> GetByEmail(string email, CancellationToken cancellationToken);
    Task <Account?> GetById(Guid id, CancellationToken cancellationToken);
    void Add(Account account);
    void Update(Account account);
}
