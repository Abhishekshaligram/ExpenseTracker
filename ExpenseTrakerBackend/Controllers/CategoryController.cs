using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeCrud.Model.Expense;
using PracticeCrud.Model;
using SITOpsBackend.Service.Common.Helper;
using PracticeCrud.Service.Category;
using PracticeCrud.Model.Category;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using PracticeCrud.Model.Login;
using PracticeCrud.Common.JwtAuthentication;
using Microsoft.AspNetCore.Authorization;

namespace PracticeCrud.Controllers
{
    [Route("api/category")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryService _categoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        public CategoryController(IConfiguration configuration,ICategoryService categoryService, IHttpContextAccessor httpContextAccessor,
            IJwtAuthenticationService jwtAuthenticationService)
        {
            _categoryService = categoryService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;   
        }

        [HttpPost("addeditcategory")]
        public async Task<ResponseModel> AddEditCategory(CategoryReqModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserId = TokenData.UserID;
            }
            BaseApiResponse response = new BaseApiResponse();
            var data = await _categoryService.AddEditCategory(model);
            if (data != null)
            {
                response.Success = true;

            }
            else
            {
                response.Success = false;
            }
            return data;

        }

        [HttpPost("getcategorylist")]
        public async Task<ApiResponse<CategroryResListModel>> GetExpenseList(CategoryListReqModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserId = TokenData.UserID;
            }
            ApiResponse<CategroryResListModel> response = new ApiResponse<CategroryResListModel>() { Data = new List<CategroryResListModel>() };
            var result = await _categoryService.GetCategoryList(model);
            if (result != null)
            {

                response.Success = true;
                response.Data = result;

            }
            else
            {
                response.Success = false;
            }
            return response;
        }


        [HttpDelete("delete/{categoryId}")]
        public async Task<ResponseModel> DeleteExpenseById(int categoryId)
        {
            ResponseModel response = new ResponseModel();
            var data = await _categoryService.DeleteCategoryById(categoryId);
            if (data != null)
            {
                response.ErrMsg = data.ErrMsg;
                response.ErrCode = data.ErrCode;

            }
            return response;

        }
    }

}
