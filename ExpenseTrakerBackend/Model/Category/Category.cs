namespace PracticeCrud.Model.Category
{
    public class Category
    {
    }

    public class CategroryResListModel
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public bool isDelete { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UserName { get; set; }
        public long TotalRecords { get; set; }

    }

    public class CategoryReqModel
    {
        public long CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
    }

    public class CategoryListReqModel : CommonRequestModel
    {
        public long UserId { get; set; }

    }
}
