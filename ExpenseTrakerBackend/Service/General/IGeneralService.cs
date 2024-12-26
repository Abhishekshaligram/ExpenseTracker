using PracticeCrud.Model;
using PracticeCrud.Model.General;
using SITOpsBackend.Service.Common.Helper;

namespace PracticeCrud.Service.General
{
    public interface IGeneralService
    {
        Task<List<GeneralUserResModel>> getUserList();
        Task<List<GeneralCategoryResModel>> getCategoryList(long userId);
        Task<List<CategoryAmountResModel>> getcategoryListforDashboard(long userId);
        Task<long> SaveProfilePicture(SaveProfilePictureModel model);
        Task<UserImageResModel> GetImageByUserId(long UserId);
        Task<ApiPostResponse<FileDownloadModel>> DownloadExpenseReportPdf(long userId);
    }
}
