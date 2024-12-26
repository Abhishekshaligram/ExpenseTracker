using PracticeCrud.Model;

namespace PracticeCrud.Common
{
    public class UserConstants
    {
        public static List<UserModel> Users = new()
        {
            new UserModel()
            {
                Username="Abhishek",
                Password="12345",
                Role="Admin"
            }
        };
    }
}
