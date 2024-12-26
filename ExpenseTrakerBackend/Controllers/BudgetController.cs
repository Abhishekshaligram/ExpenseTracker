using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PracticeCrud.Common.JwtAuthentication;
using PracticeCrud.Model.Login;
using PracticeCrud.Model;
using PracticeCrud.Service.Category;
using SITOpsBackend.Service.Common.Helper;
using PracticeCrud.Model.Budget;
using PracticeCrud.Service.Budget;
using PracticeCrud.Model.Category;
using System.Reflection;
using PracticeCrud.Common.EmailNotification;
using PracticeCrud.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace PracticeCrud.Controllers
{   
    [Route("api/budget")]
    [ApiController]
    //[Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBudgetService _budgetService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        private readonly SmtpSettings _smtpSettings;
        public BudgetController(IConfiguration configuration, IBudgetService budgeservice, IHttpContextAccessor httpContextAccessor,
            IJwtAuthenticationService jwtAuthenticationService, IOptions<SmtpSettings> smtpSettings)
        {
            _budgetService = budgeservice;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
             _smtpSettings = smtpSettings.Value;
        }
                
        [HttpPost("addeditbudget")]
        public async Task<ResponseModel> AddEditBudget(BudgetreqModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserID = TokenData.UserID;
            }
            BaseApiResponse response = new BaseApiResponse();
            var data = await _budgetService.AddEditBudget(model);
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

        [HttpPost("getbudgetlist")]
        public async Task<ApiResponse<BudgetListResponseModel>> GetBudgetList(BudgetListReqModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserID = TokenData.UserID;
            }
            ApiResponse<BudgetListResponseModel> response = new ApiResponse<BudgetListResponseModel>() { Data = new List<BudgetListResponseModel>() };
            var result = await _budgetService.GetbudgetList(model);
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
        [HttpDelete("delete/{budgetId}")]
        public async Task<ResponseModel> DeleteExpenseById(int budgetId)
        {
            ResponseModel response = new ResponseModel();
            var data = await _budgetService.DeleteBudgetById(budgetId);
            if (data != null)
            {
                response.ErrMsg = data.ErrMsg;
                response.ErrCode = data.ErrCode;

            }
            return response;

        }


        [HttpPost("getexpenseforcard")]
        public async Task<ApiResponse<ExpenseCardResponseModel>> GetExpeneForCard(long userId)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                userId = TokenData.UserID;
            }
            ApiResponse<ExpenseCardResponseModel> response = new ApiResponse<ExpenseCardResponseModel>() { Data = new List<ExpenseCardResponseModel>() };
            var data = await _budgetService.GetExpeneForCard(userId);
            if (data != null)
            {
                response.Success = true;
                response.Data = data;
            }
            else
            {
                response.Success = false;
            }
            return response;
        }

    }
}
