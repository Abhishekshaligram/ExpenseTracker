using NLog;

namespace PracticeCrud.Model.Budget
{
    public class BudgetModel
    {
    }

    public class BudgetreqModel
    {
        public long BudgetID { get; set; }
        public long UserID { get; set; }
        public long CategoryID { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class  BudgetListReqModel:CommonRequestModel
    {
        public long UserID { get; set; }
        public long CategoryID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class BudgetListResponseModel
    {
        public long BudgetID { get; set; }
        public long UserID { get; set; }
        public long CategoryID { get; set; }
        public decimal Amount { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long TotalRecords { get; set; }

    }

    public class ExpenseCardResponseModel
    {
        public decimal TotalBudget { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal RemainingBudget { get; set; }
        public string userName { get; set; }
        public string Email { get; set; }
    }

    }
