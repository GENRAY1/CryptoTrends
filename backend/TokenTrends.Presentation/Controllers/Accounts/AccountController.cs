using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenTrends.Application.Account.Get;
using TokenTrends.Application.Account.Login;
using TokenTrends.Application.Account.Logout;
using TokenTrends.Application.Account.RefreshToken;
using TokenTrends.Application.Account.Register;

namespace TokenTrends.Presentation.Controllers.Accounts;

[Route("api/[action]")]
[ApiController]
public class AccountController(ISender sender)
   : ControllerBase 
{
   [AllowAnonymous]
   [HttpPost]
   public async Task<ActionResult> Login(
      LoginAccountRequest request,
      CancellationToken cancellationToken)
   {
      var result = await sender.Send(new LoginAccountCommand
      {
         Email = request.Email,
         Password = request.Password
      }, cancellationToken);

      if (result.IsFailure)
         return BadRequest(result.Error); 
      

      return Ok(result.Value);
   }
   
   [AllowAnonymous]
   [HttpPost]
   public async Task<ActionResult> Register(
      [FromBody] RegisterAccountRequest request,
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(new RegisterAccountCommand
      {
         Email = request.Email,
         Password = request.Password,
         Username = request.Username
      }, cancellationToken);
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok();
   }
   
   [AllowAnonymous]
   [HttpPost]
   public async Task<ActionResult> RefreshToken(
      [FromBody] RefreshTokenRequest request,
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(new RefreshTokenCommand
      {
         AccessToken = request.AccessToken,
         RefreshToken = request.RefreshToken
      }, cancellationToken);

      if(result.IsFailure)
         return BadRequest(result.Error);
      
      return Ok(result.Value);
   }
   
   [Authorize]
   [HttpPost]
   public async Task<ActionResult> Logout(
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(new LogoutAccountCommand(), cancellationToken); 
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok();
   }
   
   [Authorize]
   [HttpGet]
   public async Task<ActionResult> Account(
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(
         new GetAccountQuery(), cancellationToken); 
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok(result.Value);
   }
}