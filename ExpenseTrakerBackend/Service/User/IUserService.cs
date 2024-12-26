using PracticeCrud.Model;


namespace PracticeCrud.Service.User
{
    public interface IUserService
    {
        Task<ResponseModel> AddEditUser(UserRequestModel model);
        Task<List<UserListResModel>> GetUserList(CommonRequestModel model);
        Task<ResponseModel> DeleteuserById(int userId);
    }
}
