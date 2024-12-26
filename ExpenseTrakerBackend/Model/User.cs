using Microsoft.Identity.Client;

namespace PracticeCrud.Model
{
    public class User
    {
        public string Username {  get; set; }
        public string Password { get; set; }

    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserRequestModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserListResModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public long TotalRecords { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserImageResModel
    {
        public string? Images { get; set; }
        public string? ImageUrl { get; set; }
    }
   
}
