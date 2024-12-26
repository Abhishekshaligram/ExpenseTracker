using Dapper;
using PracticeCrud.Common.Config;
using PracticeCrud.Model.Expense;
using PracticeCrud.Model;
using System.Data;
using PracticeCrud.Model.Category;

namespace PracticeCrud.Repository.Category
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IConfiguration _configuration;

        public CategoryRepository(IConfiguration configuration,IBaseRepository baseRepository)
        {
            _configuration = configuration;
            _baseRepository = baseRepository;
        }

        public async  Task<ResponseModel> AddEditCategory(CategoryReqModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@categoryId", model.CategoryID);
                param.Add("@categoryName", model.CategoryName);
                param.Add("@description", model.Description);
                param.Add("@userid", model.UserId);
                var result = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_AddEditCategories", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> DeleteCategoryById(int categoryId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@categoryId", categoryId);
                var data = await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("SP_Expense_CategoryDeleteById", param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message;
            }
            return response;
        }

        public async Task<List<CategroryResListModel>> GetCategoryList(CategoryListReqModel model)
        {
            var param = new DynamicParameters();
            param.Add("@pageNumber", model.PageNumber);
            param.Add("@pageSize", model.PageSize);
            param.Add("@sortOrder", model.sortOrder);
            param.Add("@sortColumn", model.sortColumn);
            param.Add("@UserID", model.UserId);
            var result = await _baseRepository.QueryAsync<CategroryResListModel>("SP_Expense_GetCategoryList", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}
