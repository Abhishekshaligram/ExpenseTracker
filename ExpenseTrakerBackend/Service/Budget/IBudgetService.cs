using PracticeCrud.Model.Category;
using PracticeCrud.Model;
using PracticeCrud.Model.Budget;

namespace PracticeCrud.Service.Budget
{
    public interface IBudgetService
    {
        Task<ResponseModel> AddEditBudget(BudgetreqModel model);
        Task<List<BudgetListResponseModel>> GetbudgetList(BudgetListReqModel model);
        Task<ResponseModel> DeleteBudgetById(int budgetID);
        Task<List<ExpenseCardResponseModel>> GetExpeneForCard(long userId);
       
    }
}
