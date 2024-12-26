using PracticeCrud.Model;
using PracticeCrud.Repository;
using SITOpsBackend.Service.Common.Helper;
using System.Text;
using OfficeOpenXml;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace PracticeCrud.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConfiguration _config;
        public EmployeeService(IEmployeeRepository employeeRepository, IConfiguration config)
        {
            _employeeRepository = employeeRepository;
            _config = config;
        }


        public async Task<List<DepartmentResModel>> GetDepartmentList()
        {
            return await _employeeRepository.GetDepartmentList();
        }

        public async Task<EmployeeResponseModel> GetEmployeeById(long empId)
        {
            return await _employeeRepository.GetEmployeeById(empId);
        }

        public async Task<List<EmployeeResponseListModel>> GetEmployeeList(CommonRequestModel Model)
        {
            return await _employeeRepository.GetEmployeeList(Model);
        }

        public async Task<ResponseModel> SaveEmployee(EmployeeRequestModel Model)
        {
            return await _employeeRepository.SaveEmployee(Model);

        }
        public async Task<ApiPostResponse<FileDownloadModel>> DownloadEmployeeReport(CommonRequestModel model)
        {
            var retData = new ApiPostResponse<FileDownloadModel>();
            var retDataStringBase64 = new StringBuilder();
            var FileName = string.Empty;
            // model.PageNumber = -1;
            var result = await _employeeRepository.GetEmployeeList(model);
            if (result != null)
            {
                if (result.Count > 0)
                {

                    var exportData = result.Select(x => new EmployeeCsvListModel
                    {
                        First_Name = x.First_Name,
                        Last_Name = x.Last_Name,
                        Email = x.Email,
                        DepartmentName = x.DepartmentName,
                        dept_id = x.dept_id,
                        Salary = x.Salary
                    }).ToList();
                    var exportFileName = "EmployeeData";
                    var tmpData = GenerateEmployeeDetailedExcel(exportData);
                    if (tmpData != null) {
                        retDataStringBase64=new StringBuilder(tmpData);
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



        public string GenerateEmployeeDetailedExcel(List<EmployeeCsvListModel> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var pakage = new ExcelPackage())
            {
                var worksheet = pakage.Workbook.Worksheets.Add("Employee Data");
                //Add Headers
                var headers = new string[] { "First_Name", "Last_Name", "Email", "DepartmentName", "dept_id", "Salary" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cells[1, i + 1];
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;  // Make header bold
                }
                int currentRow = 2;
                foreach (var emp in data)
                {
                    worksheet.Cells[currentRow, 1].Value = emp.First_Name;
                    worksheet.Cells[currentRow, 2].Value = emp.Last_Name;
                    worksheet.Cells[currentRow, 3].Value = emp.Email;
                    worksheet.Cells[currentRow, 4].Value = emp.DepartmentName;
                    worksheet.Cells[currentRow, 5].Value = emp.dept_id;
                    worksheet.Cells[currentRow, 6].Value = emp.Salary;
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


        //public async Task<ApiPostResponse<FileDownloadModel>> DownloadEmployeeReportPdf(CommonRequestModel model)
        //{
        //    var retData = new ApiPostResponse<FileDownloadModel>();
        //    var retDataStringBase64 = new StringBuilder();
        //    var fileName = string.Empty;

        //    try
        //    {
        //        // Fetch employee data
        //        var employeeList = await _employeeRepository.GetEmployeeList(model);

        //        if (employeeList != null && employeeList.Count > 0)
        //        {
        //            // Create a new PDF document
        //            var pdfDocument = new PdfDocument();
        //            var page = pdfDocument.AddPage();
        //            var gfx = XGraphics.FromPdfPage(page);

        //            // Define fonts
        //            var titleFont = new XFont("Arial", 20, XFontStyle.Bold);
        //            var headerFont = new XFont("Arial", 12, XFontStyle.Bold);
        //            var bodyFont = new XFont("Arial", 10, XFontStyle.Regular);

        //            // Add title
        //            gfx.DrawString("Employee Report", titleFont, XBrushes.Black,
        //                new XRect(0, 30, page.Width, 50), XStringFormats.TopCenter);

        //            // Define table structure
        //            int yPoint = 100;
        //            int margin = 40;
        //            int rowHeight = 20;

        //            // Draw table headers with a background color
        //            //gfx.DrawRectangle(XBrushes.LightGray, margin, yPoint - 10, page.Width - margin * 2, rowHeight);
        //            gfx.DrawString("First Name", headerFont, XBrushes.Black, new XPoint(margin + 10, yPoint));
        //            gfx.DrawString("Last Name", headerFont, XBrushes.Black, new XPoint(margin + 110, yPoint));
        //            gfx.DrawString("Email", headerFont, XBrushes.Black, new XPoint(margin + 210, yPoint));
        //            gfx.DrawString("Department", headerFont, XBrushes.Black, new XPoint(margin + 410, yPoint));
        //            gfx.DrawString("Salary", headerFont, XBrushes.Black, new XPoint(margin + 510, yPoint));

        //            yPoint += rowHeight;

        //            // Add employee details
        //            foreach (var employee in employeeList)
        //            {
        //                gfx.DrawString(employee.First_Name, bodyFont, XBrushes.Black, new XPoint(margin + 10, yPoint));
        //                gfx.DrawString(employee.Last_Name, bodyFont, XBrushes.Black, new XPoint(margin + 110, yPoint));
        //                gfx.DrawString(employee.Email, bodyFont, XBrushes.Black, new XPoint(margin + 210, yPoint));
        //                gfx.DrawString(employee.DepartmentName, bodyFont, XBrushes.Black, new XPoint(margin + 410, yPoint));
        //                gfx.DrawString(employee.Salary.ToString("C"), bodyFont, XBrushes.Black, new XPoint(margin + 510, yPoint));
        //                yPoint += rowHeight;

        //                // Add a new page if content overflows
        //                if (yPoint > page.Height - 50)
        //                {
        //                    page = pdfDocument.AddPage();
        //                    gfx = XGraphics.FromPdfPage(page);
        //                    yPoint = 50; // Reset yPoint for the new page

        //                    // Redraw table headers on the new page
        //                   // gfx.DrawRectangle(XBrushes.LightGray, margin, yPoint - 10, page.Width - margin * 2, rowHeight);
        //                    gfx.DrawString("First Name", headerFont, XBrushes.Black, new XPoint(margin + 10, yPoint));
        //                    gfx.DrawString("Last Name", headerFont, XBrushes.Black, new XPoint(margin + 110, yPoint));
        //                    gfx.DrawString("Email", headerFont, XBrushes.Black, new XPoint(margin + 210, yPoint));
        //                    gfx.DrawString("Department", headerFont, XBrushes.Black, new XPoint(margin + 410, yPoint));
        //                    gfx.DrawString("Salary", headerFont, XBrushes.Black, new XPoint(margin + 510, yPoint));
        //                    yPoint += rowHeight;
        //                }
        //            }

        //            // Serialize the PDF to Base64
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                pdfDocument.Save(memoryStream, false);
        //                var pdfBytes = memoryStream.ToArray();
        //                var base64Pdf = Convert.ToBase64String(pdfBytes);

        //                // Populate the response
        //                retDataStringBase64.Append(base64Pdf);
        //                fileName = "EmployeeReport.pdf";
        //            }

        //            retData = new ApiPostResponse<FileDownloadModel>
        //            {
        //                Success = true,
        //                Data = new FileDownloadModel
        //                {
        //                    FileName = fileName,
        //                    FileData = retDataStringBase64.ToString()
        //                }
        //            };
        //        }
        //        else
        //        {
        //            // Handle case where no data is found
        //            retData = new ApiPostResponse<FileDownloadModel>
        //            {
        //                Success = false,
        //                Message = "No employee data found."
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions
        //        retData = new ApiPostResponse<FileDownloadModel>
        //        {
        //            Success = false,
        //            Message = $"An error occurred: {ex.Message}"
        //        };
        //    }

        //    return retData;
        //}

        public async Task<ApiPostResponse<FileDownloadModel>> DownloadEmployeeReportPdf(CommonRequestModel model)
        {
            var retData = new ApiPostResponse<FileDownloadModel>();
            var retDataStringBase64 = new StringBuilder();
            var fileName = string.Empty;

            try
            {
                // Fetch employee data
                var employeeList = await _employeeRepository.GetEmployeeList(model);

                if (employeeList != null && employeeList.Count > 0)
                {
                    // Create a new PDF document
                    var pdfDocument = new PdfDocument();
                    var page = pdfDocument.AddPage();
                    var gfx = XGraphics.FromPdfPage(page);

                    // Define fonts
                    var titleFont = new XFont("Arial", 20, XFontStyle.Bold);
                    var headerFont = new XFont("Arial", 12, XFontStyle.Bold);
                    var bodyFont = new XFont("Arial", 10, XFontStyle.Regular);
                    var footerFont = new XFont("Arial", 8, XFontStyle.Regular);

                    // Draw logo
                    var logoPath = _config["ImagePath:UserProfileImage"]; 
                    if (File.Exists(logoPath))
                    {
                        var logo = XImage.FromFile(logoPath);
                        gfx.DrawImage(logo, 40, 20, 100, 50); 
                    }

                    // Add title
                    gfx.DrawString("Employee Report", titleFont, XBrushes.Black,
                        new XRect(0, 80, page.Width, 50), XStringFormats.TopCenter);

                    // Define table structure
                    int yPoint = 140;
                    int margin = 40;
                    int rowHeight = 20;

                    // Draw table headers
                    gfx.DrawString("First Name", headerFont, XBrushes.Black, new XPoint(margin + 10, yPoint));
                    gfx.DrawString("Last Name", headerFont, XBrushes.Black, new XPoint(margin + 110, yPoint));
                    gfx.DrawString("Email", headerFont, XBrushes.Black, new XPoint(margin + 210, yPoint));
                    gfx.DrawString("Department", headerFont, XBrushes.Black, new XPoint(margin + 410, yPoint));
                    gfx.DrawString("Salary", headerFont, XBrushes.Black, new XPoint(margin + 510, yPoint));
                    yPoint += rowHeight;

                    // Add employee details
                    foreach (var employee in employeeList)
                    {
                        gfx.DrawString(employee.First_Name, bodyFont, XBrushes.Black, new XPoint(margin + 10, yPoint));
                        gfx.DrawString(employee.Last_Name, bodyFont, XBrushes.Black, new XPoint(margin + 110, yPoint));
                        gfx.DrawString(employee.Email, bodyFont, XBrushes.Black, new XPoint(margin + 210, yPoint));
                        gfx.DrawString(employee.DepartmentName, bodyFont, XBrushes.Black, new XPoint(margin + 410, yPoint));
                        gfx.DrawString(employee.Salary.ToString("C"), bodyFont, XBrushes.Black, new XPoint(margin + 510, yPoint));
                        yPoint += rowHeight;

                        // Add a new page if content overflows
                        if (yPoint > page.Height - 100) // Adjust margin to make space for footer
                        {
                            DrawFooter(gfx, footerFont, page, pdfDocument.PageCount);
                            page = pdfDocument.AddPage();
                            gfx = XGraphics.FromPdfPage(page);
                            yPoint = 140; // Reset yPoint for the new page

                            // Redraw table headers
                            gfx.DrawString("First Name", headerFont, XBrushes.Black, new XPoint(margin + 10, yPoint));
                            gfx.DrawString("Last Name", headerFont, XBrushes.Black, new XPoint(margin + 110, yPoint));
                            gfx.DrawString("Email", headerFont, XBrushes.Black, new XPoint(margin + 210, yPoint));
                            gfx.DrawString("Department", headerFont, XBrushes.Black, new XPoint(margin + 410, yPoint));
                            gfx.DrawString("Salary", headerFont, XBrushes.Black, new XPoint(margin + 510, yPoint));
                            yPoint += rowHeight;
                        }
                    }

                    // Draw footer for the last page
                    DrawFooter(gfx, footerFont, page, pdfDocument.PageCount);

                    // Serialize the PDF to Base64
                    using (var memoryStream = new MemoryStream())
                    {
                        pdfDocument.Save(memoryStream, false);
                        var pdfBytes = memoryStream.ToArray();
                        var base64Pdf = Convert.ToBase64String(pdfBytes);

                        // Populate the response
                        retDataStringBase64.Append(base64Pdf);
                        fileName = "EmployeeReport.pdf";
                    }

                    retData = new ApiPostResponse<FileDownloadModel>
                    {
                        Success = true,
                        Data = new FileDownloadModel
                        {
                            FileName = fileName,
                            FileData = retDataStringBase64.ToString()
                        }
                    };
                }
                else
                {
                    // Handle case where no data is found
                    retData = new ApiPostResponse<FileDownloadModel>
                    {
                        Success = false,
                        Message = "No employee data found."
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                retData = new ApiPostResponse<FileDownloadModel>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }

            return retData;
        }

        // Footer drawing method
        private void DrawFooter(XGraphics gfx, XFont footerFont, PdfPage page, int pageCount)
        {
            var footerText = $"Generated by [Shaligram Infotech] | Page {pageCount}";
            gfx.DrawString(footerText, footerFont, XBrushes.Gray,
                new XRect(0, page.Height - 30, page.Width, 20), XStringFormats.TopCenter);
        }

        public async Task<TTSResponseModel> ConvertTextToSpeech(TTSRequestModel model)
        {
            return await _employeeRepository.ConvertTextToSpeech(model);
        }
    }
}
