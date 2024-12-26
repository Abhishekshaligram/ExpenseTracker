using Dapper;
using PracticeCrud.Common.Config;
using PracticeCrud.Common.JwtAuthentication;
using PracticeCrud.Model;
using PracticeCrud.Model.Budget;
using PracticeCrud.Model.Category;
using System.Data;
using System.Reflection;

namespace PracticeCrud.Repository.Budget
{

    public class BudgetRepository : IBudgetRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository _baseRepository;
       
  
        public BudgetRepository(IConfiguration configuration, IBaseRepository baseRepository)
        {
            _configuration = configuration;
            _baseRepository = baseRepository;
        }
        public async Task<ResponseModel> AddEditBudget(BudgetreqModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@BudgetID", model.BudgetID);
                param.Add("@UserID", model.UserID);
                param.Add("@CategoryID", model.CategoryID);
                param.Add("@BudgetAmount", model.Amount);
                param.Add("@StartDate", model.StartDate);
                param.Add("@EndDate", model.EndDate);
                var result = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_AddEditBudget", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        
        public async  Task<ResponseModel> DeleteBudgetById(int budgetID)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@budgetId", budgetID);
                var data = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_BudgetDeleteById", param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<List<BudgetListResponseModel>> GetbudgetList(BudgetListReqModel model)
        {
            var param = new DynamicParameters();
            param.Add("@pageNumber", model.PageNumber);
            param.Add("@pageSize", model.PageSize);
            param.Add("@sortOrder", model.sortOrder);
            param.Add("@sortColumn", model.sortColumn);
            param.Add("@UserID", model.UserID);
            param.Add("@CategoryID", model.CategoryID);
            param.Add("@StartDate", model.StartDate);
            param.Add("@EndDate", model.EndDate);
            var result = await _baseRepository.QueryAsync<BudgetListResponseModel>("SP_Expense_GetBudgetList", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async  Task<List<ExpenseCardResponseModel>> GetExpeneForCard(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserID", userId);
            var result = await _baseRepository.QueryAsync<ExpenseCardResponseModel>("SP_GetBudgetAnalysis", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }


        //public Task CheckAndSendLowBudgetNotifications()
        //{
        //    var usersWithLowBudgets = await _baseRepository.QueryAsync<ExpenseCardResponseModel>("SP_GetUsersWithLowBudgets", commandType: CommandType.StoredProcedure);
        //    return result.ToList();
           

        //}
    }
}
