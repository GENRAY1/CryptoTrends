using TokenTrends.Domain.Account.Identity.Enums;

namespace TokenTrends.Domain.Account.Identity;

public interface IConfirmationCodeRepository
{
    void Add(ConfirmationCode code);
    Task<int> GetCountFromLastHour(Guid accountId, ConfirmationCodeEvent codeEvent);
    Task<ConfirmationCode?> GetByCode(string code, CancellationToken cancellationToken);
    Task<ConfirmationCode?> GetLatestByAccountId(Guid accountId, ConfirmationCodeEvent codeEvent, CancellationToken cancellationToken);
    void Update(ConfirmationCode code);
}