using System.ComponentModel.DataAnnotations;

namespace PracticeCrud.Model.Login
{
    public class loginModel
    {
    }

    public class LoginRequestModel
    {
        [Required(ErrorMessage = "Email id required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password required!")]
        public string Password { get; set; }
    }
    public class UserLoginResponseModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public string Token { get; set; }
        public string? Images { get; set; } // for later
       
    }
    public class HashResponseModel
    {
        public string? PasswordHash { get; set; }
        public string? Password { get; set; }
        public bool? IsDelete { get; set; }
        public int? Status { get; set; }
    }

    public class TokenModel
    {
        public long tokenId { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime TokenValidTo { get; set; }
        public string? Images { get; set; }
        public string Token { get; set; }
    }

    public class AccessTokenModel
    {
        public string? Token { get; set; }
        public int ValidityInMin { get; set; }
        public DateTime ExpiresOnUTC { get; set; }
        public long UserID { get; set; }
       
    }
    public class ForgetPasswordResponseModel
    {
        public long UserID { get; set; }

        public DateTime LastForgetPasswordSend { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime ChangePasswordUrlEmailLinkExpireIn { get; set; }
    }

    public class ForgetPasswordRequestModel
    {
        [Required]
        public string Email { get; set; }
        public string ForgetPasswordUrl { get; set; }
    }

    public class PasswordChangeRequestModel
    {
        public string? EncryptedUserId { get; set; }
        public DateTime? EncryptedDateTime { get; set; }
        public string? DateTime { get; set; }
        public string? EncryptedCustomerId { get; set; }
        public string? Password { get; set; }
        public long? UserID { get; set; }
        public long? CustomerId { get; set; }
        public string? PasswordHash { get; set; }
        public bool? IsButtonClicked { get; set; }
    }
}
