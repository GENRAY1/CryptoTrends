using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TokenTrends.Application.Abstractions.Services.Authentication;
using TokenTrends.Application.Services.Authentication;
using TokenTrends.Infrastructure.Services.Authentication.Exceptions;

namespace TokenTrends.Infrastructure.Services.Authentication;

public class AccountContext(IHttpContextAccessor contextAccessor) : IAccountContext
{
    public Guid AccountId
    {
        get
        {
            var value = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (value != null)
                return Guid.Parse(value);
            
            throw new AccountNotAuthenticatedException();
        }
    }
}

