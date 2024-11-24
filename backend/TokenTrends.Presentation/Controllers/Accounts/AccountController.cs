using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TokenTrends.Application.Account.ForgotPassword;
using TokenTrends.Application.Account.Get;
using TokenTrends.Application.Account.Login;
using TokenTrends.Application.Account.Logout;
using TokenTrends.Application.Account.Photos.Delete;
using TokenTrends.Application.Account.Photos.Upload;
using TokenTrends.Application.Account.RefreshToken;
using TokenTrends.Application.Account.Register;
using TokenTrends.Application.Account.ResetPassword;

namespace TokenTrends.Presentation.Controllers.Accounts;

[Route("api/")]
[ApiController]
public class AccountController(ISender sender)
   : ControllerBase 
{
   [AllowAnonymous]
   [HttpPost("login")]
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
   [HttpPost("register")]
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
   [HttpPost("refreshToken")]
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
   [HttpPost("logout")]
   public async Task<ActionResult> Logout(
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(new LogoutAccountCommand(), cancellationToken); 
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok();
   }
   
   [HttpPost("forgot-password")]
   public async Task<ActionResult> ForgotPassword(
      [FromBody] ForgotPasswordRequest request,
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(new ForgotPasswordCommand
      {
         Email = request.Email
      }, cancellationToken); 
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok();
   }
   
   [HttpPost("reset-password")]
   public async Task<ActionResult> ResetPassword(
      [FromBody] ResetPasswordRequest request,
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(new ResetPasswordCommand
      {
         Code = request.Code,
         NewPassword = request.NewPassword
      }, cancellationToken); 
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok(result.Value);
   }
   
   [Authorize]
   [HttpGet("account")]
   public async Task<ActionResult> Account(
      CancellationToken cancellationToken) 
   {
      var result = await sender.Send(
         new GetAccountQuery(), cancellationToken); 
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok(result.Value);
   }

   [Authorize]
   [HttpPost("account/photo")]
   public async Task<ActionResult> UploadAccountPhoto(
      IFormFile photo,
      CancellationToken cancellationToken)
   {
      await using var stream = photo.OpenReadStream();

      var command = new UploadAccountPhotoCommand
      {
         FileName = photo.FileName,
         FileStream = stream
      };
      
      var result = await sender.Send(command, cancellationToken);
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok();
   }
   
   [Authorize]
   [HttpDelete("account/photo")]
   public async Task<ActionResult> DeleteAccountPhoto(
      string fileName,
      CancellationToken cancellationToken)
   {
      var command = new DeleteAccountPhotoCommand
      {
         FileName = fileName
      };
      
      var result = await sender.Send(command, cancellationToken);
      
      if(result.IsFailure)
         return BadRequest(result.Error);

      return Ok();
   }
}