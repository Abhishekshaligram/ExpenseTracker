using Dapper;
using Microsoft.Extensions.Options;
using PracticeCrud.Common.Config;
using PracticeCrud.Model;
using System.Data;

namespace PracticeCrud.Repository.User
{
    public class UserRepository: IUserRepository
    {
        public readonly IOptions<DataConfig> _connectionString;
        private readonly IConfiguration _config;
        private IBaseRepository _baseRepository;
        public UserRepository(IOptions<DataConfig> connectionString,IConfiguration configuration, IBaseRepository baseRepository) 
        { 
           _connectionString = connectionString;
            _config = configuration;
            _baseRepository = baseRepository;
        }
        public async Task<ResponseModel> AddEditUser(UserRequestModel model)
        {
             ResponseModel response = new ResponseModel();
            try
            {
             var param = new DynamicParameters();
             param.Add("@userid", model.UserID);
             param.Add("@userName", model.UserName);
             param.Add("@email", model.Email);
             param.Add("@isactive", model.IsActive);
             var result = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_AddEditUser", param, commandType: CommandType.StoredProcedure);
             return result;
            }
            catch (Exception ex) {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> DeleteuserById(int userId)
        {
            ResponseModel response= new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@userid", userId);
                var data = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_UserDelete", param, commandType: CommandType.StoredProcedure);
                return data;
            }catch(Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<List<UserListResModel>> GetUserList(CommonRequestModel model)
        {
                var param= new DynamicParameters();
                param.Add("@pageNumber",model.PageNumber);
                param.Add("@pageSize", model.PageSize);
                param.Add("@sortOrder", model.sortOrder);
                param.Add("@sortColumn", model.sortColumn);
                var result= await _baseRepository.QueryAsync<UserListResModel>("SP_Expense_GetUserList",param, commandType: CommandType.StoredProcedure);
                return result.ToList();   
        }
    }
}
