using PracticeCrud.Model;
using SITOpsBackend.Service.Common.Helper;

namespace PracticeCrud.Repository
{
    public interface IEmployeeRepository
    {
        Task<EmployeeResponseModel> GetEmployeeById(long empId);
        Task<ResponseModel> SaveEmployee(EmployeeRequestModel Model);
        Task<List<EmployeeResponseListModel>> GetEmployeeList(CommonRequestModel Model);
        Task<List<DepartmentResModel>> GetDepartmentList();
        Task<TTSResponseModel> ConvertTextToSpeech(TTSRequestModel model);

    }
}
