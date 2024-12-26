using PracticeCrud.Model.Budget;
using PracticeCrud.Model;

namespace PracticeCrud.Repository.Budget
{
    public interface IBudgetRepository
    {
        Task<ResponseModel> AddEditBudget(BudgetreqModel model);
        Task<List<BudgetListResponseModel>> GetbudgetList(BudgetListReqModel model);
        Task<ResponseModel> DeleteBudgetById(int budgetID);
        Task<List<ExpenseCardResponseModel>> GetExpeneForCard(long userId);
        //Task<ResponseModel> CheckAndSendLowBudgetNotifications();
    }
}
