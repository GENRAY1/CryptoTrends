using PetPalsProfile.Domain.Absractions;

namespace TokenTrends.Domain.Account;

public static class AccountErrors
{
   public static Error InvalidCredentials = new("AccountError.InvalidCredentials", "Invalid email or password");

   public static Error NotLoggedIn = new ("AccountError.NotLoggedIn", "Account not logged in");

   public static Error AccountNotFound = new("AccountError.AccountNotFound", "Account not found");

   public static Error EmailAlreadyExists = new("AccountError.EmailAlreadyExists", "Email already exists");
   
   public static Error AccessTokenInvalid = new("AccountError.AccessTokenInvalid", "Access token is invalid");
   
   public static Error RefreshTokenNotActive = new ("AccountError.RefreshTokenNotActive", "Refresh token not active");

   public static Error RefreshTokenExpired = new ("AccountError.RefreshTokenExpired", "Refresh token expired. Login again");

   public static Error InvalidRefreshToken  =new ("AccountError.InvalidRefreshToken", "Invalid refresh token");
   
   public static Error RefreshTokenNotFound = new("AccountError.RefreshTokenNotFound", "Refresh token not found");
}