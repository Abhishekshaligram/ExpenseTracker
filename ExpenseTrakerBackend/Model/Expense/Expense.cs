namespace PracticeCrud.Model.Expense
{
    public class Expense
    {
    }

    public class ExpenseListResModel
    {
        public long ExpenseID { get; set; }
        public long UserID { get; set; }
        public long CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long UpdatedBy { get; set; }
        public long TotalRecords { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
    }

    public class ExpenseRequestModel
    {
        public long ExpenseId { get; set; }
        public long UserId { get; set; }
        public long CategoryId { get; set; }
        public string Amount { get; set; } = string.Empty;
        public string Description { get; set; }
    }

    public class ExpenseByCategoryReqModel : CommonRequestModel
    {
        public long UserID { get; set; }
    }
    public class ExpenseByCategoryResponseModel
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public long TotalRecords { get; set; }
    }

    public class MonthExpenseReqModel : CommonRequestModel
    {
        public long UserID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }


    public class MonthExpenseResponseModel 
    {  
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public long TotalTransactions { get; set; }
        public long userID { get; set; }
        public string userName { get; set; }
        public long TotalRecords { get; set; }
    }

    public class MontlyExpenseReportModel
    {
        public string userName { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
        public long TotalTransactions { get; set; }
    }


    public class ExpenseByCategoryForGraphResponseModel
    {
        public string CategoryName { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ExpenseReqModel : CommonRequestModel
    {
        public long UserID { get; set; }
    }
}
