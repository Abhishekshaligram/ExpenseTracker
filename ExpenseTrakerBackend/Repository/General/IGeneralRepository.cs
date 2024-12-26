using PracticeCrud.Model;
using PracticeCrud.Model.General;

namespace PracticeCrud.Repository.General
{
    public interface IGeneralRepository
    {
        Task<List<GeneralUserResModel>> getUserList();
        Task<List<GeneralCategoryResModel>> getCategoryList(long userId);
        Task<List<CategoryAmountResModel>> getcategoryListforDashboard(long userId);
        Task<long> SaveProfilePicture(SaveProfilePictureModel model);
        Task<UserImageResModel> GetImageByUserId(long UserId);
    }
}
