using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using PracticeCrud.Model;
using PracticeCrud.Service.User;
using SITOpsBackend.Service.Common.Helper;

namespace PracticeCrud.Controllers
{
    [Route("api/user")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public UserController(IConfiguration configuration,IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost("addedituser")]
        public async Task<ResponseModel> AddEditUser(UserRequestModel model) { 
          BaseApiResponse response = new BaseApiResponse();
            var data= await _userService.AddEditUser(model);
            if (data != null) { 
              response.Success = true;

            }
            else
            {
                response.Success = false;
            }
            return data;
        
        }

        [HttpPost("getuserlist")]

        public async Task<ApiResponse<UserListResModel>> GetUserList(CommonRequestModel model)
        {
            ApiResponse<UserListResModel> response = new ApiResponse<UserListResModel>() { Data=new List<UserListResModel>() };
            var result= await _userService.GetUserList(model);
            if (result != null) { 
            
             response.Success=true;
             response.Data=result;
            
            }
            else
            {
                response.Success=false;
            }
            return response;
        }
        [HttpDelete("delete/{userId}")]
        public async Task<ResponseModel>DeleteuserById(int userId)
        {
            ResponseModel response = new ResponseModel();
            var data= await _userService.DeleteuserById(userId);
            if (data != null) { 
              response.ErrMsg=data.ErrMsg;
              response.ErrCode=data.ErrCode;
            
            }
            return response ;

        }

    }
}
