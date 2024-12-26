using Dapper;
using PracticeCrud.Common.Config;
using PracticeCrud.Model;
using PracticeCrud.Model.Login;
using System.Data;

namespace PracticeCrud.Repository.Login
{
   
    public class LoginRepository:ILoginRepository
    {
        private readonly IBaseRepository _baseRepository;
        public LoginRepository(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public async  Task<string> ChangePasswordForUser(long UserId, string Password, string Salt)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            param.Add("@NewPassword", Password);
            param.Add("@PasswordSalt", Salt);
            var data = await _baseRepository.QueryFirstOrDefaultAsync<string>("SP_UpdatePasswordByUser", param, commandType: CommandType.StoredProcedure);
            return data;
        }

        public async Task<ForgetPasswordResponseModel> ForgetPasswordForUser(string Email)
        {
            var param = new DynamicParameters();
            param.Add("@Email", Email);
            return await _baseRepository.QueryFirstOrDefaultAsync<ForgetPasswordResponseModel>("SP_GetUserIdByEmail", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<HashResponseModel> GetUserHashByEmail(string Email)
        {
            var param = new DynamicParameters();
            param.Add("@Email", Email);
            return await _baseRepository.QueryFirstOrDefaultAsync<HashResponseModel>("SP_GetHashByEmail", param, commandType: CommandType.StoredProcedure);
        }

        public async  Task<long> LogoutUser(string token)
        {
            var param = new DynamicParameters();
            param.Add("@Token", token);
            return await _baseRepository.QueryFirstOrDefaultAsync<long>("SP_LogoutUser", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> SuccessForgetPasswordChangeURL(PasswordChangeRequestModel passwordInfo)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", passwordInfo.UserID);
            param.Add("@Password", passwordInfo.Password);
            param.Add("@PasswordSalt", passwordInfo.PasswordHash);
            param.Add("@EncryptedDateTime", passwordInfo.EncryptedDateTime);
            param.Add("@IsButtonClicked", passwordInfo.IsButtonClicked);
            return await _baseRepository.QueryFirstOrDefaultAsync<long>("SP_ForgetPasswordChangeWithURL", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<long> UpdateLoginToken(string Token, long UserID)
        {
            var param = new DynamicParameters();
            param.Add("@Token", Token);
            param.Add("@UserId", UserID);
            return await _baseRepository.QueryFirstOrDefaultAsync<long>("SP_UpdateLoginToken", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<UserLoginResponseModel> UserLogin(string email)
        {
            var param = new DynamicParameters();
            param.Add("@Email", email);
            return await _baseRepository.QueryFirstOrDefaultAsync<UserLoginResponseModel>("SP_UserLogin", param, commandType: CommandType.StoredProcedure);
        }
    }
}
