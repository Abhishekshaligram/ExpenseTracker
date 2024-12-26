using PracticeCrud.Model.Expense;
using PracticeCrud.Model;

namespace PracticeCrud.Repository.Expense
{
    public interface IExpenseRepository
    {
        Task<ResponseModel> AddEditExpense(ExpenseRequestModel model);
        Task<List<ExpenseListResModel>> GetExpenseList(ExpenseReqModel model);
        Task<ResponseModel> DeleteExpenseById(int expenseId);
        Task<List<ExpenseByCategoryResponseModel>> GetExpenseByCategoryList(ExpenseByCategoryReqModel model);
        Task<List<MonthExpenseResponseModel>> GetMonthExpenseList(MonthExpenseReqModel model);
        Task<List<ExpenseByCategoryForGraphResponseModel>> GetExpenseByCategoryListForGraph(long userId);
    }
}
