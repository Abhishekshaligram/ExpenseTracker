﻿using PracticeCrud.Model.Login;

namespace PracticeCrud.Service.Login
{
    public interface ILoginService
    {
        Task<HashResponseModel> GetUserHashByEmail(string Email);
        Task<UserLoginResponseModel> UserLogin(string email);
        Task<long> UpdateLoginToken(string Token, long UserID);
        Task<ForgetPasswordResponseModel> ForgetPasswordForUser(string Email);
        Task<long> SuccessForgetPasswordChangeURL(PasswordChangeRequestModel passwordInfo);
        Task<string> ChangePasswordForUser(long UserId, string Password, string Salt);
        Task<long> LogoutUser(string token);
    }
}
