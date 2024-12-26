using Dapper;
using Microsoft.Extensions.Options;
using PracticeCrud.Common.Config;
using PracticeCrud.Model;
using System.Data;
using System.Speech.Synthesis;
using System.IO;

namespace PracticeCrud.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly IConfiguration _config;
        private IHttpContextAccessor _httpContextAccessor;
        private IBaseRepository _baseRepository;
        public readonly IOptions<DataConfig> _connectionString;
        public EmployeeRepository(IOptions<DataConfig> connectionString, IConfiguration config, IHttpContextAccessor httpContextAccessor, IBaseRepository baseRepository) {
            _config=config;
            _httpContextAccessor=httpContextAccessor;
            _baseRepository=baseRepository;
            _connectionString=connectionString;

        }
        public async Task<EmployeeResponseModel> GetEmployeeById(long empId)
        {
            var param = new DynamicParameters();
            param.Add("@empId", empId);
            var result = await _baseRepository.QueryFirstOrDefaultAsync<EmployeeResponseModel>("sp_getEmployeeById", param, commandType: CommandType.StoredProcedure);
            await _baseRepository.CloseConnAsync();
           return result;
        }

        public async Task<ResponseModel> SaveEmployee(EmployeeRequestModel Model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@empId", Model.empId);
                param.Add("@firstname", Model.First_Name);
                param.Add("@lastname", Model.Last_Name);
                param.Add("@email", Model.Email);
                param.Add("@salary", Model.Salary);
                param.Add("@deptId", Model.dept_id);
                //param.Add("@deptId", Model.dept_id);
                //param.Add("@dateofbirth", Model.dateofBirth);
                response= await _baseRepository.QueryFirstOrDefaultAsync<ResponseModel>("sp_AddEditEmployee",param,commandType: CommandType.StoredProcedure);
                await _baseRepository.CloseConnAsync();
            }
            catch (Exception ex) {
                response.ErrCode = 102;
                response.ErrMsg = ex.Message.ToString();
            
            }
            return response;

        }

        public async Task<List<EmployeeResponseListModel>> GetEmployeeList(CommonRequestModel Model)
        {
                var param=new DynamicParameters();
                param.Add("@pageNumber", Model.PageNumber);
                param.Add("@pageSize", Model.PageSize);
                param.Add("@sortOrder", Model.sortOrder);
                param.Add("@sortColumn", Model.sortColumn);
                var result = await _baseRepository.QueryAsync<EmployeeResponseListModel>("GetEmployeeList",param, commandType: CommandType.StoredProcedure);
                return result.ToList();
        }

        public async Task<List<DepartmentResModel>> GetDepartmentList()
        {
            var param = new DynamicParameters();
            var result = await _baseRepository.QueryAsync<DepartmentResModel>("SP_GetDepartmentList", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<TTSResponseModel> ConvertTextToSpeech(TTSRequestModel model)
        {
            TTSResponseModel response = new TTSResponseModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("@Text", model.Text);
                param.Add("@FilePath", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
                response = await _baseRepository.QueryFirstOrDefaultAsync<TTSResponseModel>(
                    "InsertTTSRecord", param, commandType: CommandType.StoredProcedure
                );

                var filePath = param.Get<string>("@FilePath");

                if (!string.IsNullOrEmpty(filePath))
                {
                    var fullPath = Path.Combine("wwwroot", filePath);
                    var directoryPath = Path.GetDirectoryName(fullPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    using (var synthesizer = new SpeechSynthesizer())
                    {
                        synthesizer.SetOutputToWaveFile(fullPath);
                        synthesizer.Speak(model.Text);
                    }

                    response.Success = true;
                    response.Data = new { FilePath = filePath };
                }
                else
                {
                    response.Success = false;
                    response.ErrCode = 102;
                    response.ErrMsg = "Failed to generate the TTS audio file.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrCode = 500;
                response.ErrMsg = $"An error occurred while processing TTS: {ex.Message}";
            }

            return response;
        }

    }
}
