using PracticeCrud.Model;
using PracticeCrud.Repository.User;

namespace PracticeCrud.Service.User
{
    public class UserService: IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public UserService(IConfiguration configuration, IUserRepository userRepository)
        {
            configuration=_configuration;
            _userRepository=userRepository;
        }
        public async  Task<ResponseModel> AddEditUser(UserRequestModel model)
        {
           return await _userRepository.AddEditUser(model);
        }

        public async Task<ResponseModel> DeleteuserById(int userId)
        {
          return  await _userRepository.DeleteuserById(userId);
        }

        public async Task<List<UserListResModel>> GetUserList(CommonRequestModel model)
        {
            return await _userRepository.GetUserList(model);
        }
    }
}
