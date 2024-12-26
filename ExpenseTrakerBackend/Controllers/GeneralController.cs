using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PracticeCrud.Common.CommonMethods;
using PracticeCrud.Model.General;
using PracticeCrud.Service.General;
using SITOpsBackend.Service.Common.Helper;
using PracticeCrud.Common.Enum;
using PracticeCrud.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using PracticeCrud.Model.Login;
using System.Reflection;
using PracticeCrud.Common.JwtAuthentication;
using Microsoft.AspNetCore.Authorization;
namespace PracticeCrud.Controllers
{
    [Route("api/general")]
    [ApiController]
    //[Authorize]
    public class GeneralController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IGeneralService _generalService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        public GeneralController(IConfiguration configuration,IGeneralService generalService, IHttpContextAccessor httpContextAccessor, IJwtAuthenticationService jwtAuthenticationService)
        {
            _configuration = configuration;
            _generalService = generalService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
        }
        [HttpGet("getcategorylist/{userId}")]
        public async Task<ApiResponse<GeneralCategoryResModel>> getCategoryList(long userId)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                userId = TokenData.UserID;
            }
            ApiResponse<GeneralCategoryResModel> response = new ApiResponse<GeneralCategoryResModel> { Data = new List<GeneralCategoryResModel>() };
            var result = await _generalService.getCategoryList(userId);

            if (result != null)
            {
                response.Data = result;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return response;
        }
        [HttpGet("getuserlist")]
        public async Task<ApiResponse<GeneralUserResModel>> getUserList()
        {
            ApiResponse<GeneralUserResModel> response = new ApiResponse<GeneralUserResModel> { Data = new List<GeneralUserResModel>() };
            var result = await _generalService.getUserList();

            if (result != null)
            {
                response.Data = result;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return response;
        }

        [HttpGet("getcategoryfordashboard/{userId}")]
        public async Task<ApiResponse<CategoryAmountResModel>> getCategoryForDashboard(long userId)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                userId = TokenData.UserID;
            }
            ApiResponse<CategoryAmountResModel> response = new ApiResponse<CategoryAmountResModel> { Data = new List<CategoryAmountResModel>() };
            var result = await _generalService.getcategoryListforDashboard(userId);

            if (result != null)
            {
                response.Data = result;
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return response;
        }

        [HttpPost("SaveUserProfile")]
        public async Task<BaseApiResponse> SaveUserProfile([FromForm] SaveProfilePictureModel model)
        {

            BaseApiResponse response = new BaseApiResponse();
            var result = await _generalService.SaveProfilePicture(model);   
            if (result == Convert.ToInt16(SaveResult.Inserted))
            {
                response.Success = true;
                response.Message = ErrorMessages.UserInsertedSuccess;
            }
            else if (result == Convert.ToInt16(SaveResult.Updated))
            {
                response.Success = true;
                response.Message = ErrorMessages.UserUpdateSuccess;
            }
            else if (result == Convert.ToInt16(SaveResult.SqlException))
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
            }

            return response;
        }

        [HttpGet("details/{userId}")]
        public async Task<ApiPostResponse<UserImageResModel>> GetImageByUserId(long userId)
        {
            ApiPostResponse<UserImageResModel> response = new ApiPostResponse<UserImageResModel>();
            var result = await _generalService.GetImageByUserId(userId);
            if (result != null)
            {
                response.Success = true;
                response.Data = result;
            }
            else
            {
                response.Success = false;
                response.Message = ErrorMessages.SomethingWentWrong;
            }
            return response;
        }


        [HttpPost("expensepdf")]
        public async Task<IActionResult> DownloadExpenseStatementPdf(long userId)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                userId = TokenData.UserID;
            }
            return Ok(await _generalService.DownloadExpenseReportPdf(userId));

        }
    }
}
