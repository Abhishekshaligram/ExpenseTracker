using PracticeCrud.Model;

namespace PracticeCrud.Repository.User
{
    public interface IUserRepository
    {
        Task<ResponseModel> AddEditUser(UserRequestModel model);
        Task<List<UserListResModel>> GetUserList(CommonRequestModel model);
        Task<ResponseModel> DeleteuserById(int userId);
    }
}
