
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PracticeCrud.Common.CommonMethods;
using PracticeCrud.Common.Enum;
using PracticeCrud.Common.Helper;
using PracticeCrud.Common.JwtAuthentication;
using PracticeCrud.Common.Settings;
using PracticeCrud.Model.Login;
using PracticeCrud.Service.Login;
using SITOpsBackend.Service.Common.Helper;
using PracticeCrud.Common.EmailNotification;
using PracticeCrud.Common.Methods;
using System.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using PracticeCrud.Model;
using Microsoft.AspNetCore.Authorization;

namespace PracticeCrud.Controllers
{
    [Route("api/login")]
    [ApiController]
    //[Authorize]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginService _loginservice;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly SmtpSettings _smtpSettings;
        public LoginController(IConfiguration configuration, ILoginService loginService, IHttpContextAccessor httpContextAccessor,
             IOptions<AppSettings> appSettings, IJwtAuthenticationService jwtAuthenticationService, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
                 IOptions<SmtpSettings> smtpSettings)
        {
            _configuration = configuration;
            _loginservice = loginService;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
            _jwtAuthenticationService = jwtAuthenticationService;
            _hostingEnvironment = hostingEnvironment;
            _smtpSettings = smtpSettings.Value;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ApiPostResponse<UserLoginResponseModel>> Login([FromBody] LoginRequestModel model)
            {
            ApiPostResponse<UserLoginResponseModel> response = new ApiPostResponse<UserLoginResponseModel>() { Data = new UserLoginResponseModel() };
            model.Password = EncryptionDecryption.GetEncrypt(model.Password);
            HashResponseModel res = await _loginservice.GetUserHashByEmail(model.Email);
            if (res.Status == LoginStatus.UserDeactive)
            {
                response.Success = false;
                response.Message = ErrorMessages.AccountDeactivated;
                return response;
            }
            else if (res.Status == LoginStatus.UserDeleted)
            {
                response.Success = false;
                response.Message = ErrorMessages.UserIsDeletedByAdmin;
                return response;
            }
            else if (res.Status == LoginStatus.EmailNotExist)
            {
                response.Success = false;
                response.Message = ErrorMessages.EmailNotExist;
                return response;
            }
            else if (res == null)
            {
                response.Success = false;
                response.Message = ErrorMessages.InvalidEmailId;
                return response;
            }
            else
            {
                string Hash = EncryptionDecryption.GetDecrypt(res.Password);
                string Salt = EncryptionDecryption.GetDecrypt(res.PasswordHash);
                bool isPasswordMatched = EncryptionDecryption.Verify(model.Password, Hash, Salt);
                var Path = _httpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
                if (isPasswordMatched)
                {
                    model.Password = res.Password;
                    UserLoginResponseModel result = await _loginservice.UserLogin(model.Email);
                    if (result != null && result.UserID > 0)
                    {
                        TokenModel objTokenData = new TokenModel();
                        objTokenData.Email = model.Email;
                        objTokenData.UserID = result.UserID != null ? result.UserID : 0;
                        objTokenData.UserName = result.UserName;
                        objTokenData.TokenValidTo = DateTime.UtcNow.AddMinutes(_appSettings.JWT_Validity_Mins);
                        objTokenData.Images = Path + _configuration["ImagePath:UserProfileImage"] + "/" + result.Images;
                        AccessTokenModel objAccessTokenData = _jwtAuthenticationService.GenerateToken(objTokenData, _appSettings.JWT_Secret, _appSettings.JWT_Validity_Mins);
                        result.Token = objAccessTokenData.Token;
                        await _loginservice.UpdateLoginToken(objAccessTokenData.Token, objTokenData.UserID);
                        response.Message = ErrorMessages.LoginSuccess;
                        response.Success = true;
                        response.Data = result;
                        #region Set default image for user if not exist on server
                        var IsImageExist = _hostingEnvironment.WebRootPath + _configuration["ImagePath:UserProfileImage"] + '/' + result.Images;
                        response.Data.Images = Path + _configuration["ImagePath:UserProfileImage"] + "/" + result.Images;
                        string staticImageUrl = Path + _configuration["ImagePath:UserProfileImage"] + "/" + "profile.png";
                        response.Data.Images = !string.IsNullOrEmpty(IsImageExist) && System.IO.File.Exists(IsImageExist) ? result.Images : staticImageUrl;
                        #endregion
                        return response;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = ErrorMessages.InvalidCredential;
                        return response;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = ErrorMessages.InvalidCredential;
                    return response;
                }

            }
        }
        [HttpPost("forget-password")]
        public async Task<BaseApiResponse> ForgetPasswordWithURL([FromBody] ForgetPasswordRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            string emailBody;
            string EncryptedCustomerId;
            string EncryptedDateTime;
            var result = await _loginservice.ForgetPasswordForUser(model.Email);
            if (result == null)
                {
                    response.Message = ErrorMessages.NotRegisterEmailId;
                    response.Success = false;
                }       
                else if (result.UserID > 0)
                {
                    EncryptedCustomerId = HttpUtility.UrlEncode(EncryptionDecryption.GetEncryptNew(result.UserID.ToString()));
                    EncryptedDateTime = HttpUtility.UrlEncode(EncryptionDecryption.GetEncryptNew((result.ChangePasswordUrlEmailLinkExpireIn).ToString()));
                    EmailSetting setting = new EmailSetting
                    {
                        EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                        EmailHostName = _smtpSettings.EmailHostName,
                        EmailPassword = _smtpSettings.EmailPassword,
                        EmailAppPassword = _smtpSettings.EmailAppPassword,
                        EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                        FromEmail = _smtpSettings.FromEmail,
                        FromName = _smtpSettings.FromName,
                        EmailUsername = _smtpSettings.EmailUsername,
                    };
                 string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.EmailTemplate);
                 if (!Directory.Exists(BasePath))
                 {
                    Directory.CreateDirectory(BasePath);
                 }
                 bool isSuccess = false;
                 using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.ForgetPasswordEmailtem)))
                 {
                    emailBody = reader.ReadToEnd();
                 }
                 var path = _httpContextAccessor.HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
                 var passwordUrl = model.ForgetPasswordUrl + '/' + EncryptedCustomerId + '/' + EncryptedDateTime;
                emailBody = emailBody.Replace("##Logo##", path + _configuration["Images:ShaligramLogo"]);
                emailBody = emailBody.Replace("##userName##", (result.UserName));
                emailBody = emailBody.Replace("##FooterLogoURL##", path + _configuration["Images:ShaligramLogo"]);
                emailBody = emailBody.Replace("##currentYear##", DateTime.Now.Year.ToString());
                emailBody = emailBody.Replace("##PrivacyPolicyURL##", "");
                emailBody = emailBody.Replace("##TermsConditionsURL##", "");
                emailBody = emailBody.Replace("##Password##", passwordUrl);
                emailBody = emailBody.Replace("##currentYear##", DateTime.Now.Year.ToString());
                isSuccess = await Task.Run(() => EmailNotifications.SendMailMessage(model.Email, null, null, "Reset password link", emailBody, setting, null));
                if (isSuccess)
                {
                    response.Message = ErrorMessages.ForgetPasswordSuccessEmail;
                    response.Success = true;
                }
                else
                {   
                    response.Message = ErrorMessages.EmailVerifyOrWait;
                    response.Success = true;
                }
            }
            else
            {
                response.Message = ErrorMessages.Error;
                response.Success = false;
                Response.StatusCode = StatusCodes.Status404NotFound;
            }
            return response;
        }
        [HttpPost("success-forget-password")]
        public async Task<BaseApiResponse> SuccessForgetPasswordChangeURL([FromBody] PasswordChangeRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            string decryptedId = EncryptionDecryption.GetDecryptNew(HttpUtility.UrlDecode(model.EncryptedUserId));
            string decryptedDateTime = EncryptionDecryption.GetDecryptNew(HttpUtility.UrlDecode(model.DateTime));
            model.UserID = Convert.ToInt64(decryptedId);
            model.EncryptedDateTime = Convert.ToDateTime(decryptedDateTime);
            string[] encryptedPassword = CommonMethods.GenerateEncryptedPasswordAndPasswordSalt(model.Password);
            model.Password = encryptedPassword[0];
            model.PasswordHash = encryptedPassword[1];
            var result = await _loginservice.SuccessForgetPasswordChangeURL(model);
            if (result == Status.URLExpired)
            {
                response.Success = false;
                response.Message = ErrorMessages.UrlForPasswordChangeExpired;
            }
            else if (result == Status.URLUsed)
            {
                response.Success = false;
                response.Message = ErrorMessages.URLAlreadyUsed;
            }
            else if (result == Status.IsOnLoadCheck)
            {
                response.Success = true;
                response.Message = ErrorMessages.UrlIsValid;
            }
            else if (result == Status.Success)
            {
                response.Success = true;
                response.Message = ErrorMessages.ResetPasswordSuccess;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }

        [HttpPost("change-password")]
        public async Task<BaseApiResponse> UpdatePasswordByCustomer([FromBody] ChangePasswordRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            TokenModel tokenModel = new TokenModel();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                tokenModel = _jwtAuthenticationService.GetTokenData(jwtToken);
            }
            model.OldPassword = EncryptionDecryption.GetEncrypt(model.OldPassword.Trim());
            model.CreatePassword = EncryptionDecryption.GetEncrypt(model.CreatePassword.Trim());
            model.ConfirmPassword = EncryptionDecryption.GetEncrypt(model.ConfirmPassword.Trim());
            var res = await _loginservice.GetUserHashByEmail(tokenModel.Email);
            bool isPasswordSame = true;
            bool isPasswordMatched = true;
            if (tokenModel.UserID == 0 || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.CreatePassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                response.Message = ErrorMessages.PasswordFieldValidation;
                response.Success = false;
                return response;
            }
            if (res != null)
            {
                string Hash = EncryptionDecryption.GetDecrypt(res.Password);
                string Salt = EncryptionDecryption.GetDecrypt(res.PasswordHash);
                isPasswordMatched = EncryptionDecryption.Verify(model.OldPassword, Hash, Salt);
                isPasswordSame = EncryptionDecryption.Verify(model.CreatePassword, Hash, Salt);
            }
            #region Validation 
            if (!isPasswordMatched)
            {
                response.Message = ErrorMessages.PasswordCheck;
                response.Success = false;
                return response;
            }
            if (isPasswordSame)
            {
                response.Message = ErrorMessages.PasswordMatch;
                response.Success = false;
                return response;
            }
            if (string.IsNullOrEmpty(model.CreatePassword) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                response.Message = ErrorMessages.PasswordValidation;
                response.Success = false;
                return response;
            }
            if (model.CreatePassword != model.ConfirmPassword)
            {
                response.Message = ErrorMessages.ConfirmPassword;
                response.Success = false;
                return response;
            }
            if (!CommonMethods.IsPasswordStrong(EncryptionDecryption.GetDecrypt(model.CreatePassword)))
            {
                response.Message = ErrorMessages.StrongPassword;
                response.Success = false;
                return response;
            }
            #endregion
            string hashed = EncryptionDecryption.Hash(model.CreatePassword);
            string[] segments = hashed.Split(":");
            string EncryptedHash = EncryptionDecryption.GetEncrypt(segments[0]);
            string EncryptedSalt = EncryptionDecryption.GetEncrypt(segments[1]);
            var result = await _loginservice.ChangePasswordForUser(tokenModel.UserID, EncryptedHash, EncryptedSalt);
            if (string.IsNullOrEmpty(result))
            {
                response.Message = ErrorMessages.ResetPasswordSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
                Response.StatusCode = StatusCodes.Status403Forbidden;
            }
            return response;
        }

        [HttpPost("logout")]
        public async Task<BaseApiResponse> Logout()
            {
            BaseApiResponse response = new BaseApiResponse();
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
            var data = await _loginservice.LogoutUser(jwtToken);
            if (data > 0)
            {
                response.Message = ErrorMessages.UserLogoutSuccess;
                response.Success = true;
            }
            else
            {
                response.Message = ErrorMessages.SomethingWentWrong;
                response.Success = false;
            }
            return response;
        }
    }
}
