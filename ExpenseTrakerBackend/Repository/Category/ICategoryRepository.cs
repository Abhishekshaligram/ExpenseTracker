using PracticeCrud.Model.Category;
using PracticeCrud.Model;

namespace PracticeCrud.Repository.Category
{
    public interface ICategoryRepository
    {
        Task<ResponseModel> AddEditCategory(CategoryReqModel model);
        Task<List<CategroryResListModel>> GetCategoryList(CategoryListReqModel model);
        Task<ResponseModel> DeleteCategoryById(int categoryId);
    }
}
