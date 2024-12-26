using OfficeOpenXml;
using PracticeCrud.Model;
using PracticeCrud.Model.Expense;
using PracticeCrud.Repository.Expense;
using SITOpsBackend.Service.Common.Helper;
using System.Reflection;
using System.Text;

namespace PracticeCrud.Service.Expense
{
    public class ExpenseService : IExpenseService
    {
        private readonly IConfiguration _configuration;
        private readonly IExpenseRepository _expenseRepository;
        public ExpenseService(IConfiguration configuration,IExpenseRepository expenseRepository)
        {
             _configuration = configuration;
            _expenseRepository = expenseRepository;
        }
        public async Task<ResponseModel> AddEditExpense(ExpenseRequestModel model)
        {
            return await _expenseRepository.AddEditExpense(model);
        }

        public async Task<ResponseModel> DeleteExpenseById(int expenseId)
        {
            return await _expenseRepository.DeleteExpenseById(expenseId);
        }

        public async Task<List<ExpenseByCategoryResponseModel>> GetExpenseByCategoryList(ExpenseByCategoryReqModel model)
        {
            return await _expenseRepository.GetExpenseByCategoryList(model);
        }

        public async Task<List<ExpenseListResModel>> GetExpenseList(ExpenseReqModel model)
        {
            return await _expenseRepository.GetExpenseList(model);
        }

        public async Task<List<MonthExpenseResponseModel>> GetMonthExpenseList(MonthExpenseReqModel model)
        {
           return await _expenseRepository.GetMonthExpenseList(model);
        }

        public async Task<ApiPostResponse<FileDownloadModel>> DownloadMonthlyExpenseReport(MonthExpenseReqModel model)
        {
            var retData = new ApiPostResponse<FileDownloadModel>();
            var retDataStringBase64 = new StringBuilder();
            var FileName = string.Empty;
            // model.PageNumber = -1;
            var result = await _expenseRepository.GetMonthExpenseList(model);
            if (result != null) {
                if (result.Count > 0) {
                    var exportData = result.Select(x => new MontlyExpenseReportModel
                    {
                        userName=x.userName,
                        CategoryName = x.CategoryName,
                        TotalAmount = x.TotalAmount,
                        TotalTransactions = x.TotalTransactions,
                    }).ToList();
                    var exportFileName = "MonthlyExpenseReport";
                    var tmpData = GenerateEmployeeDetailedExcel(exportData);
                    if (tmpData != null)
                    {
                        retDataStringBase64 = new StringBuilder(tmpData);
                        FileName = $"{exportFileName}.xlsx";
                    }
                    retData = new ApiPostResponse<FileDownloadModel>
                    {
                        Success = true,
                        Data = new FileDownloadModel
                        {
                            FileName = FileName,
                            FileData = Convert.ToString(retDataStringBase64)
                        }
                    };

                }
                else
                {
                    retData = new ApiPostResponse<FileDownloadModel>
                    {
                        Success = false,
                    };
                }
            }
            else
            {
                retData = new ApiPostResponse<FileDownloadModel>
                {
                    Success = false,
                };
            }
            return retData;
        }

        public string GenerateEmployeeDetailedExcel(List<MontlyExpenseReportModel> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var pakage = new ExcelPackage())
            {
                var worksheet = pakage.Workbook.Worksheets.Add("MothlyExpenseData");
                //Add Headers
                var headers = new string[] { "userName","CategoryName", "TotalAmount", "TotalTransactions"};
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cells[1, i + 1];
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;  // Make header bold
                }
                int currentRow = 2;
                foreach (var emp in data)
                {
                    worksheet.Cells[currentRow, 1].Value = emp.userName;
                    worksheet.Cells[currentRow, 2].Value = emp.CategoryName;
                    worksheet.Cells[currentRow, 3].Value = emp.TotalAmount;
                    worksheet.Cells[currentRow, 4].Value = emp.TotalTransactions;
                    currentRow++;
                }
                // Save to a memory stream
                using (var stream = new MemoryStream())
                {
                    pakage.SaveAs(stream);
                    var byteArray = stream.ToArray();
                    return Convert.ToBase64String(byteArray);
                }
            }    
        }

        public async Task<List<ExpenseByCategoryForGraphResponseModel>> GetExpenseByCategoryListForGraph(long userId)
        {
           return await _expenseRepository.GetExpenseByCategoryListForGraph(userId);
        }
    }
}
