using System.ComponentModel.DataAnnotations;

namespace PracticeCrud.Model
{
    public class EmployeeResponseModel
    {
        public long empId { get; set; }
        public string Full_Name { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTime? createdDate { get; set; }
    }


    public class EmployeeRequestModel
    {
        public long empId { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public decimal Salary { get; set; }
        public long dept_id { get; set; }
  
    }
    public class ResponseModel
    {
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; } = string.Empty;
    }

    public class EmployeeResponseListModel
    {
        public long empId { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public int TotalRecords { get; set; }
        public long dept_id { get; set; }
        public decimal Salary { get; set; }
    }

    public class CommonRequestModel
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? sortColumn { get; set; }
        public string? sortOrder { get; set; }
    }

    public class DepartmentResModel
    {
        public int? departmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? empId { get; set; }
    }

    public class EmployeeCsvListModel
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public long dept_id { get; set; }
        public decimal Salary { get; set; }
    }

    public class FileDownloadModel
    {
        public string FileName { get; set; }
        public string FileData { get; set; }
    }
    public class TTSRequestModel
    {
        public string Text { get; set; }
    }
    public class TTSResponseModel
    {
        public bool Success { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }
        public object Data { get; set; } 
    }
    public class TranslationRequest
    {
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Text { get; set; }
    }

    public class ChangePasswordRequestModel
    {
        [Required(ErrorMessage = "The Old Password is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "The Create Password is required.")]
        public string CreatePassword { get; set; }
        [Required(ErrorMessage = "The Confirm Password is required.")]
        public string ConfirmPassword { get; set; }
    }
}
