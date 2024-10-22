using MediatR;
using Microsoft.AspNetCore.Mvc;
using TokenTrends.Application.Account.Login;

namespace TokenTrends.Presentation.Controllers;

[Route("api/[action]")]
[ApiController]
public class AccountController(ISender sender)
   : ControllerBase 
{
   [HttpPost]
   public async Task<ActionResult> Login(CancellationToken cancellationToken)
   {
      await sender.Send(new LoginAccountCommand(), cancellationToken);
      
      return Ok();
   }
   
}