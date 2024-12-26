using PracticeCrud.Model;
using PracticeCrud.Model.Expense;
using SITOpsBackend.Service.Common.Helper;

namespace PracticeCrud.Service.Expense
{
    public interface IExpenseService
    {
        Task<ResponseModel> AddEditExpense(ExpenseRequestModel model);
        Task<List<ExpenseListResModel>> GetExpenseList(ExpenseReqModel model);
        Task<ResponseModel> DeleteExpenseById(int expenseId);
        Task<List<ExpenseByCategoryResponseModel>> GetExpenseByCategoryList(ExpenseByCategoryReqModel model);
        Task<List<MonthExpenseResponseModel>> GetMonthExpenseList(MonthExpenseReqModel model);
        Task<ApiPostResponse<FileDownloadModel>> DownloadMonthlyExpenseReport(MonthExpenseReqModel model);
        Task<List<ExpenseByCategoryForGraphResponseModel>> GetExpenseByCategoryListForGraph(long userId);
    }
}
