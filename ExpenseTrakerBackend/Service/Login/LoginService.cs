using PracticeCrud.Model.Login;
using PracticeCrud.Repository.Login;

namespace PracticeCrud.Service.Login
{
    public class LoginService:ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository) 
        { 
          _loginRepository = loginRepository;
        }

        public async Task<string> ChangePasswordForUser(long UserId, string Password, string Salt)
        {
            return await _loginRepository.ChangePasswordForUser(UserId, Password, Salt);
        }

        public async Task<ForgetPasswordResponseModel> ForgetPasswordForUser(string Email)
        {
           return await _loginRepository.ForgetPasswordForUser(Email);
        }

        public async Task<HashResponseModel> GetUserHashByEmail(string Email)
        {
            return await _loginRepository.GetUserHashByEmail(Email);
        }

        public async Task<long> LogoutUser(string token)
        {
            return await _loginRepository.LogoutUser(token);
        }

        public async  Task<long> SuccessForgetPasswordChangeURL(PasswordChangeRequestModel passwordInfo)
        {
            return await _loginRepository.SuccessForgetPasswordChangeURL(passwordInfo);
        }

        public async Task<long> UpdateLoginToken(string Token, long UserID)
        {
           return await _loginRepository.UpdateLoginToken(Token, UserID);
        }

        public async Task<UserLoginResponseModel> UserLogin(string email)
        {
            return await _loginRepository.UserLogin(email);
        }
    }
}
