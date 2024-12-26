using Dapper;
using PracticeCrud.Common.Config;
using PracticeCrud.Model;
using PracticeCrud.Model.Expense;
using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PracticeCrud.Repository.Expense
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository _baseRepository;
        public ExpenseRepository(IConfiguration configuration,IBaseRepository baseRepository) 
        { 
            _configuration = configuration;
            _baseRepository = baseRepository;
        }
        public async Task<ResponseModel> AddEditExpense(ExpenseRequestModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@expenseId", model.ExpenseId);
                param.Add("@userId", model.UserId);
                param.Add("@categoryId", model.CategoryId);
                param.Add("@amount", model.Amount);
                param.Add("@description", model.Description);
                var result = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_AddEditExpenses", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> DeleteExpenseById(int expenseId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@expenseId", expenseId);
                var data = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_ExpenseDeleteById", param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<List<ExpenseByCategoryResponseModel>> GetExpenseByCategoryList(ExpenseByCategoryReqModel model)
        {
            var param = new DynamicParameters();
            param.Add("@pageNumber", model.PageNumber);
            param.Add("@pageSize", model.PageSize);
            param.Add("@sortOrder", model.sortOrder);
            param.Add("@sortColumn", model.sortColumn);
            param.Add("@UserID", model.UserID);
            var result = await _baseRepository.QueryAsync<ExpenseByCategoryResponseModel>("SP_GetTotalExpensesByCategory", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<ExpenseByCategoryForGraphResponseModel>> GetExpenseByCategoryListForGraph(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@UserID", userId);
            var result = await _baseRepository.QueryAsync<ExpenseByCategoryForGraphResponseModel>("SP_GetTotalExpensesByCategoryForGraph", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<ExpenseListResModel>> GetExpenseList(ExpenseReqModel model)
        {
            var param = new DynamicParameters();
            param.Add("@pageNumber", model.PageNumber);
            param.Add("@pageSize", model.PageSize);
            param.Add("@sortOrder", model.sortOrder);
            param.Add("@sortColumn", model.sortColumn);
            param.Add("@userId", model.UserID);
            var result = await _baseRepository.QueryAsync<ExpenseListResModel>("SP_Expense_GetExpenseList", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<List<MonthExpenseResponseModel>> GetMonthExpenseList(MonthExpenseReqModel model)
        {
            var param = new DynamicParameters();
            param.Add("@pageNumber", model.PageNumber);
            param.Add("@pageSize", model.PageSize);
            param.Add("@sortOrder", model.sortOrder);
            param.Add("@sortColumn", model.sortColumn);
            param.Add("@UserID", model.UserID);
            param.Add("@Year", model.Year);
            param.Add("@Month", model.Month);
            var result = await _baseRepository.QueryAsync<MonthExpenseResponseModel>("SP_GetMonthlyExpensesSummary", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}
