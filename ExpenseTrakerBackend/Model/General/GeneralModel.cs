namespace PracticeCrud.Model.General
{
    public class GeneralModel
    {
    }

    public class GeneralUserResModel
    {
        public long UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
    public class GeneralCategoryResModel
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
    }

    public class CategoryAmountResModel
    {
        public long ExpenseID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class SaveProfilePictureModel
    {
        public long UserId { get; set; }
        public string? Images { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }

    public class SaveUserResponseModel
    {
        public long Status { get; set; }
        public string OldImageName { get; set; }
        public string NewImageName { get; set; }

    }

    public class NotificationResModel
    {
        public int NotificationId {get; set; }
        public int UserId { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
