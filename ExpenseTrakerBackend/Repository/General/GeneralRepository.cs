using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using PracticeCrud.Common.Config;
using PracticeCrud.Common.Methods;
using PracticeCrud.Model;
using PracticeCrud.Model.General;
using System.Data;

namespace PracticeCrud.Repository.General
{

    public class GeneralRepository : IGeneralRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository _baseRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private IHttpContextAccessor _httpContextAccessor;
        public GeneralRepository(IConfiguration configuration, IBaseRepository baseRepository, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _baseRepository = baseRepository;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<GeneralCategoryResModel>> getCategoryList(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@userId", userId);
            var result = await _baseRepository.QueryAsync<GeneralCategoryResModel>("SP_getCategoryList", param, commandType: CommandType.StoredProcedure);
            return result.ToList();

        }

        public async  Task<List<CategoryAmountResModel>> getcategoryListforDashboard(long userId)
        {
            var param = new DynamicParameters();
            param.Add("@userId", userId);
            var result = await _baseRepository.QueryAsync<CategoryAmountResModel>("SP_GetCategoryListWithAmount", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<UserImageResModel> GetImageByUserId(long UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            var result = await _baseRepository.QueryFirstOrDefaultAsync<UserImageResModel>("SP_GetProfilePicture", param, commandType: CommandType.StoredProcedure);
            await _baseRepository.CloseConnAsync();
            var Imagepath = _hostEnvironment.WebRootPath + _configuration["ImagePath:UserProfileImage"];
            var Path = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
            if (result == null || string.IsNullOrEmpty(result.Images))
            {
                result = new UserImageResModel
                {
                    ImageUrl = Path + _configuration["Images:DefaultProfileImage"] // Use a single default image
                };
            }
            else
            {
                // Set the full URL of the user's image
                result.Images = Path + _configuration["ImagePath:UserProfileImage"] + '/' + result.Images;
            }

            return result;

        }

        public async  Task<List<GeneralUserResModel>> getUserList()
        {
            var result = await _baseRepository.QueryAsync<GeneralUserResModel>("SP_getUserList", commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<long> SaveProfilePicture(SaveProfilePictureModel model)
        {
            var path = _hostEnvironment.WebRootPath + _configuration["ImagePath:UserProfileImage"];
            if (model.ProfileImage != null)
            {
                model.Images = await CommonMethods.UploadImage(model.ProfileImage, path, null, true); //Save Image in API
            }
            var param = new DynamicParameters();
            param.Add("@UserId", model.UserId);
            param.Add("@Images", model.Images);

            var result = await _baseRepository.QueryFirstOrDefaultAsync<SaveUserResponseModel>("SP_SaveProfilePicture", param, commandType: CommandType.StoredProcedure);
            await _baseRepository.CloseConnAsync();

            if (result.OldImageName != null || result.OldImageName != "")
            {
                DirectoryInfo d = new DirectoryInfo(path);
                FileInfo[] Files = d.GetFiles();
                foreach (FileInfo file in Files)
                {
                    if (file.Name == result.OldImageName)
                    {
                        // Delete the file
                        file.Delete();
                        break; // Exit loop since file is deleted
                    }
                }
            }
            return result.Status;
        }
    }
}
