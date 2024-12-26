using PracticeCrud.Model;
using SITOpsBackend.Service.Common.Helper;

namespace PracticeCrud.Service
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseModel> GetEmployeeById(long empId);
        Task<ResponseModel> SaveEmployee(EmployeeRequestModel Model);
        Task<List<EmployeeResponseListModel>> GetEmployeeList(CommonRequestModel Model);
        Task<List<DepartmentResModel>> GetDepartmentList();
        Task<ApiPostResponse<FileDownloadModel>> DownloadEmployeeReport(CommonRequestModel model);
        Task<ApiPostResponse<FileDownloadModel>> DownloadEmployeeReportPdf(CommonRequestModel model);
        Task<TTSResponseModel> ConvertTextToSpeech(TTSRequestModel model);

    }
}
