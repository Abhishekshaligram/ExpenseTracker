using PracticeCrud.Model;
using PracticeCrud.Model.Category;
using PracticeCrud.Repository.Category;
using System.Reflection;

namespace PracticeCrud.Service.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(IConfiguration configuration, ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
                _configuration = configuration;
        }
        public async  Task<ResponseModel> AddEditCategory(CategoryReqModel model)
        {
            return await _categoryRepository.AddEditCategory(model);
        }

        public async Task<ResponseModel> DeleteCategoryById(int categoryId)
        {
            return await _categoryRepository.DeleteCategoryById(categoryId);
        }

        public async Task<List<CategroryResListModel>> GetCategoryList(CategoryListReqModel model)
        {
            return await _categoryRepository.GetCategoryList(model);
        }
    }
}
