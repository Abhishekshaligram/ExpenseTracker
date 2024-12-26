using PracticeCrud.Model;
using PracticeCrud.Model.Category;

namespace PracticeCrud.Service.Category
{
    public interface ICategoryService
    {
        Task<ResponseModel> AddEditCategory(CategoryReqModel model);
        Task<List<CategroryResListModel>> GetCategoryList(CategoryListReqModel model);
        Task<ResponseModel> DeleteCategoryById(int categoryId);
    }
}
