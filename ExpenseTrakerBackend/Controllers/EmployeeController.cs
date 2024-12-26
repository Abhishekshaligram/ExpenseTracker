using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeCrud.Common.Helper;
using PracticeCrud.Model;
using PracticeCrud.Service;
using SITOpsBackend.Service.Common.Helper;
using System.Reflection;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Azure;
using PracticeCrud.Service.User;

namespace PracticeCrud.Controllers
{
    [Route("api/employee")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICSVService _csvService;
        private readonly HttpClient _httpClient;
        
        public EmployeeController(IEmployeeService employeeService, ICSVService csvService, IHttpClientFactory httpClientFactory) {
             _employeeService = employeeService;
            _csvService = csvService;
            _httpClient = httpClientFactory.CreateClient();
           
        }


        [HttpGet("getById")]
        public async Task<EmployeeResponseModel> GetEmployeeById(long empId)
        {
            var result= await _employeeService.GetEmployeeById(empId);
            return result;
        }

        [HttpPost("saveEmployee")]
        public async Task<ResponseModel> SaveEmployee(EmployeeRequestModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            var result = await _employeeService.SaveEmployee(model);
            if (result != null)
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
            }
            return result;
        }

        [HttpPost("getList")]

        public async Task<ApiResponse<EmployeeResponseListModel>> GetEmployeeList(CommonRequestModel model)
        {
            ApiResponse<EmployeeResponseListModel> response = new ApiResponse<EmployeeResponseListModel> { Data = new List<EmployeeResponseListModel>() };
            var data = await _employeeService.GetEmployeeList(model);
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

        [HttpGet("GetDepartmentList")]
        public async Task<ApiResponse<DepartmentResModel>> GetDepartmentList()
        {
            ApiResponse<DepartmentResModel> response = new ApiResponse<DepartmentResModel> { Data = new List<DepartmentResModel>() };
            var data = await _employeeService.GetDepartmentList();
            if(data != null)
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

        //public async Task<IActionResult> GetEmployeeCSV([FromForm] IFormFileCollection file)
        //{
        //    var employees = _csvService.ReadCSV<EmployeeCsvListModel>(file[0].OpenReadStream());

        //    return Ok(employees);
        //}
        [HttpPost("employeecsv")]
        public async Task<IActionResult> DownloadEmployeeReport(CommonRequestModel model)
        {
            return Ok(await _employeeService.DownloadEmployeeReport(model));

        }

        [HttpPost("employeepdf")]
        public async Task<IActionResult> DownloadEmployeeReportPdf(CommonRequestModel model)
        {
            return Ok(await _employeeService.DownloadEmployeeReportPdf(model));

        }
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertImageToPdf(IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                    return BadRequest("No file uploaded.");
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                if (Array.IndexOf(allowedExtensions, fileExtension) == -1)
                    return BadRequest("Invalid image file format. Only JPG, JPEG, and PNG are allowed.");
                PdfDocument pdfDocument = new PdfDocument();
                PdfPage page = pdfDocument.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    stream.Position = 0;
                    XImage xImage = XImage.FromStream(() => stream);
                    gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
                }
                using (var ms = new MemoryStream())
                {
                    pdfDocument.Save(ms);
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/pdf", "converted_image.pdf");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("converttexttovoice")]
        public async Task<TTSResponseModel> ConvertTextToSpeech([FromBody] TTSRequestModel model)
        {
            TTSResponseModel response = new TTSResponseModel();
            try
            {
                response = await _employeeService.ConvertTextToSpeech(model);
                if (response != null && response.Success)
                {
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.ErrCode = 500;
                response.ErrMsg = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        [HttpPost("translate")]
        public async Task<IActionResult> Translate([FromBody] TranslationRequest request)
        {
            var url = "https://libretranslate.com";
            var data = new
            {
                q = request.Text,
                source = request.SourceLanguage,
                target = request.TargetLanguage
            };
            var response = await _httpClient.PostAsJsonAsync(url, data);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Translation failed.");
            }

            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }
    }

    }

