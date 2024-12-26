using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using PracticeCrud.Common.JwtAuthentication;
using PracticeCrud.Model;
using PracticeCrud.Model.Expense;
using PracticeCrud.Model.Login;
using PracticeCrud.Service.Expense;
using SITOpsBackend.Service.Common.Helper;
using System.Reflection;

namespace PracticeCrud.Controllers
{
    [Route("api/Expense")]
    [ApiController]
    //[Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IExpenseService _expenseService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        public ExpenseController( IConfiguration configuration,IExpenseService expenseService, IHttpContextAccessor httpContextAccessor,
            IJwtAuthenticationService jwtAuthenticationService)
        {
            _configuration = configuration;
            _expenseService = expenseService;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;

        }


        [HttpPost("addeditexpense")]
        public async Task<ResponseModel> AddEditExpense(ExpenseRequestModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserId = TokenData.UserID;
            }
            BaseApiResponse response = new BaseApiResponse();
            var data = await _expenseService.AddEditExpense(model);
            if (data != null)
            {
                response.Success = true;
                response.Message=data.ErrCode.ToString();
                response.Message = data.ErrMsg.ToString();

            }
            else
            {
                response.Success = false;
            }
            return data;

        }

        [HttpPost("getexpenselist")]

        public async Task<ApiResponse<ExpenseListResModel>> GetExpenseList(ExpenseReqModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserID = TokenData.UserID;
            }
            ApiResponse<ExpenseListResModel> response = new ApiResponse<ExpenseListResModel>() { Data = new List<ExpenseListResModel>() };
            var result = await _expenseService.GetExpenseList(model);
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
            [HttpDelete("delete/{expenseId}")]
            public async Task<ResponseModel> DeleteExpenseById(int expenseId)
            {
                ResponseModel response = new ResponseModel();
                var data = await _expenseService.DeleteExpenseById(expenseId);
                if (data != null)
                {
                    response.ErrMsg = data.ErrMsg;
                    response.ErrCode = data.ErrCode;

                }
                return response;

            }

        [HttpPost("getexpensebycategory")]

        public async Task<ApiResponse<ExpenseByCategoryResponseModel>> GetExpenseByCategoryList(ExpenseByCategoryReqModel model)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserID = TokenData.UserID;
            }
            ApiResponse<ExpenseByCategoryResponseModel> response = new ApiResponse<ExpenseByCategoryResponseModel>() { Data = new List<ExpenseByCategoryResponseModel>() };
            var result = await _expenseService.GetExpenseByCategoryList(model);
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


        [HttpPost("getmonthlyexpense")]
        public async Task<ApiResponse<MonthExpenseResponseModel>> GetMonthExpenseList(MonthExpenseReqModel model)
        {

            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                model.UserID = TokenData.UserID;
            }
            ApiResponse<MonthExpenseResponseModel> response = new ApiResponse<MonthExpenseResponseModel>() { Data = new List<MonthExpenseResponseModel>() };
            var result = await _expenseService.GetMonthExpenseList(model);
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

        [HttpPost("monthlyexpensereport")]
        public async Task<IActionResult> DownloadMonthlyExpenseReport(MonthExpenseReqModel model)
        {
            return Ok(await _expenseService.DownloadMonthlyExpenseReport(model));

        }

        [HttpPost("getexpensebycategoryforgraph")]
        public async Task<ApiResponse<ExpenseByCategoryForGraphResponseModel>> GetExpenseByCategoryListForGraph(long userId)
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (jwtToken != null && jwtToken != "")
            {
                TokenModel TokenData = _jwtAuthenticationService.GetTokenData(jwtToken);
                userId = TokenData.UserID;
            }
            ApiResponse<ExpenseByCategoryForGraphResponseModel> response = new ApiResponse<ExpenseByCategoryForGraphResponseModel>() { Data = new List<ExpenseByCategoryForGraphResponseModel>() };
            var result = await _expenseService.GetExpenseByCategoryListForGraph(userId);
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


    }
 
}
