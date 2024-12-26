using PracticeCrud.Model;
using PracticeCrud.Model.General;
using PracticeCrud.Repository.General;
using SITOpsBackend.Service.Common.Helper;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
namespace PracticeCrud.Service.General

{
    public class GeneralService : IGeneralService
    {
        private readonly IConfiguration _configuration;
        private readonly IGeneralRepository _generalRepository;
        public GeneralService(IConfiguration configuration, IGeneralRepository generalRepository)
        {
            _configuration = configuration;
            _generalRepository = generalRepository;

        }


        public async Task<List<GeneralCategoryResModel>> getCategoryList(long userId)
        {
            return await _generalRepository.getCategoryList(userId);
        }

        public async Task<List<CategoryAmountResModel>> getcategoryListforDashboard(long userId)
        {
            return await _generalRepository.getcategoryListforDashboard(userId);
        }

        public async Task<UserImageResModel> GetImageByUserId(long UserId)
        {
            return await _generalRepository.GetImageByUserId(UserId);
        }

        public async Task<List<GeneralUserResModel>> getUserList()
        {
            return await _generalRepository.getUserList();
        }

        public async Task<long> SaveProfilePicture(SaveProfilePictureModel model)
        {
            return await _generalRepository.SaveProfilePicture(model);
        }


        public async Task<ApiPostResponse<FileDownloadModel>> DownloadExpenseReportPdf(long userId)
        {
            var retData = new ApiPostResponse<FileDownloadModel>();
            var fileName = string.Empty;

            try
            {
                var expenseList = await _generalRepository.getcategoryListforDashboard(userId);
                if (expenseList != null && expenseList.Any())
                {
                    var pdfFilePath = GenerateExpensePdf(expenseList);
                    var fileContent = Convert.ToBase64String(System.IO.File.ReadAllBytes(pdfFilePath));

                    retData.Success = true;
                    retData.Data = new FileDownloadModel
                    {
                        FileName = "ExpenseStatement.pdf",
                        FileData = fileContent
                    };
                }
                else
                {
                    retData.Success = false;
                    retData.Message = "No data found for the provided user ID.";
                }
            }
            catch (Exception ex)
            {
                retData.Success = false;
                retData.Message = $"Error generating the PDF: {ex.Message}";
            }

            return retData;
        }

        private string GenerateExpensePdf(List<CategoryAmountResModel> expenses)
        {
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExpenseStatements");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var pdfFilePath = Path.Combine(directoryPath, $"ExpenseStatement_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            var pdfDocument = new PdfSharpCore.Pdf.PdfDocument();
            pdfDocument.Info.Title = "Expense Statement";

            var footerFont = new XFont("Verdana", 10, XFontStyle.Italic);
            var pdfPage = pdfDocument.AddPage();
            pdfPage.Size = PdfSharpCore.PageSize.A4;

            var gfx = XGraphics.FromPdfPage(pdfPage);
            var titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
            var headerFont = new XFont("Verdana", 12, XFontStyle.Bold);
            var textFont = new XFont("Verdana", 10, XFontStyle.Regular);
            double tableStartX = 40;
            double tableStartY = 100;
            double tableWidth = pdfPage.Width - 80;
            double rowHeight = 25;
            gfx.DrawString("Expense Statement", titleFont, XBrushes.Black, new XPoint(tableStartX, 50));
            gfx.DrawString($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", textFont, XBrushes.Black, new XPoint(tableStartX, 80));

            XBrush headerBackground = XBrushes.LightGray;
            gfx.DrawRectangle(headerBackground, tableStartX, tableStartY, tableWidth, rowHeight);
            gfx.DrawString("Expense ID", headerFont, XBrushes.Black, new XPoint(tableStartX + 10, tableStartY + 17));
            gfx.DrawString("Amount", headerFont, XBrushes.Black, new XPoint(tableStartX + 100, tableStartY + 17));
            gfx.DrawString("Category Name", headerFont, XBrushes.Black, new XPoint(tableStartX + 200, tableStartY + 17));
            gfx.DrawString("Created Date", headerFont, XBrushes.Black, new XPoint(tableStartX + 320, tableStartY + 17));

            double currentY = tableStartY + rowHeight;
            bool isAlternating = false;
            foreach (var expense in expenses)
            {
                XBrush rowBackground = isAlternating ? XBrushes.White : XBrushes.LightBlue;
                isAlternating = !isAlternating;

                gfx.DrawRectangle(rowBackground, tableStartX, currentY, tableWidth, rowHeight);

                // Obfuscate and limit ExpenseID to 8 characters
                var obfuscatedExpenseID = HashAndLimitExpenseID(expense.ExpenseID.ToString());
                gfx.DrawString(obfuscatedExpenseID, textFont, XBrushes.Black, new XPoint(tableStartX + 10, currentY + 17));
                gfx.DrawString(expense.Amount.ToString("C", new CultureInfo("hi-IN")), textFont, XBrushes.Black, new XPoint(tableStartX + 100, currentY + 17));
                gfx.DrawString(expense.CategoryName, textFont, XBrushes.Black, new XPoint(tableStartX + 200, currentY + 17));
                gfx.DrawString(expense.CreatedDate.ToString("yyyy-MM-dd"), textFont, XBrushes.Black, new XPoint(tableStartX + 320, currentY + 17));

                currentY += rowHeight;
                if (currentY + rowHeight > pdfPage.Height - 100)
                {
                    gfx.DrawString("© 2024 Expense Tracker - All Rights Reserved", footerFont, XBrushes.Gray, new XPoint(pdfPage.Width / 2, pdfPage.Height - 30), XStringFormats.Center);
                    pdfPage = pdfDocument.AddPage();
                    gfx = XGraphics.FromPdfPage(pdfPage);
                    currentY = 40;
                }
            }
            gfx.DrawString("© 2024 Expense Tracker - All Rights Reserved", footerFont, XBrushes.Gray, new XPoint(pdfPage.Width / 2, pdfPage.Height - 30), XStringFormats.Center);
            pdfDocument.Save(pdfFilePath);

            return pdfFilePath;
        }

        private string HashAndLimitExpenseID(string expenseID)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(expenseID));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                return hashString.Substring(0, 8);
            }
        }




        //public async Task<ApiPostResponse<FileDownloadModel>> DownloadExpenseReportPdf(long userId)
        //{
        //    var retData = new ApiPostResponse<FileDownloadModel>();
        //    var fileName = string.Empty;

        //    try
        //    {
        //        var expenseList = await _generalRepository.getcategoryListforDashboard(userId);
        //        if (expenseList != null && expenseList.Any())
        //        {
        //            var pdfFilePath = GenerateExpensePdf(expenseList);
        //            var fileContent = Convert.ToBase64String(System.IO.File.ReadAllBytes(pdfFilePath));

        //            retData.Success = true;
        //            retData.Data = new FileDownloadModel
        //            {
        //                FileName = "ExpenseStatement.pdf",
        //                FileData = fileContent
        //            };
        //        }
        //        else
        //        {
        //            retData.Success = false;
        //            retData.Message = "No data found for the provided user ID.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retData.Success = false;
        //        retData.Message = $"Error generating the PDF: {ex.Message}";
        //    }

        //    return retData;
        //}

        //private string GenerateExpensePdf(List<CategoryAmountResModel> expenses)
        //{
        //    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ExpenseStatements");
        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }

        //    var pdfFilePath = Path.Combine(directoryPath, $"ExpenseStatement_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        //    var pdfDocument = new PdfSharpCore.Pdf.PdfDocument();
        //    pdfDocument.Info.Title = "Expense Statement";

        //    var footerFont = new XFont("Verdana", 10, XFontStyle.Italic);
        //    var pdfPage = pdfDocument.AddPage();
        //    pdfPage.Size = PdfSharpCore.PageSize.A4;

        //    var gfx = XGraphics.FromPdfPage(pdfPage);
        //    var titleFont = new XFont("Verdana", 20, XFontStyle.Bold);
        //    var headerFont = new XFont("Verdana", 12, XFontStyle.Bold);
        //    var textFont = new XFont("Verdana", 10, XFontStyle.Regular);
        //    double tableStartX = 40;
        //    double tableStartY = 100;
        //    double tableWidth = pdfPage.Width - 80;
        //    double rowHeight = 25;

        //    gfx.DrawString("Expense Statement", titleFont, XBrushes.Black, new XPoint(tableStartX, 50));
        //    gfx.DrawString($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", textFont, XBrushes.Black, new XPoint(tableStartX, 80));

        //    // Draw the header row
        //    XBrush headerBackground = XBrushes.LightGray;
        //    gfx.DrawRectangle(headerBackground, tableStartX, tableStartY, tableWidth, rowHeight);
        //    gfx.DrawString("Amount", headerFont, XBrushes.Black, new XPoint(tableStartX + 10, tableStartY + 17));
        //    gfx.DrawString("Category Name", headerFont, XBrushes.Black, new XPoint(tableStartX + 120, tableStartY + 17));
        //    gfx.DrawString("Created Date", headerFont, XBrushes.Black, new XPoint(tableStartX + 320, tableStartY + 17));
        //    gfx.DrawString("Expense ID", headerFont, XBrushes.Black, new XPoint(tableStartX + 420, tableStartY + 17));

        //    double currentY = tableStartY + rowHeight;
        //    bool isAlternating = false;

        //    foreach (var expense in expenses)
        //    {
        //        XBrush rowBackground = isAlternating ? XBrushes.White : XBrushes.LightBlue;
        //        isAlternating = !isAlternating;

        //        gfx.DrawRectangle(rowBackground, tableStartX, currentY, tableWidth, rowHeight);
        //        gfx.DrawString(expense.Amount.ToString("C", new CultureInfo("hi-IN")), textFont, XBrushes.Black, new XPoint(tableStartX + 10, currentY + 17));
        //        gfx.DrawString(expense.CategoryName, textFont, XBrushes.Black, new XPoint(tableStartX + 120, currentY + 17));
        //        gfx.DrawString(expense.CreatedDate.ToString("yyyy-MM-dd"), textFont, XBrushes.Black, new XPoint(tableStartX + 320, currentY + 17));
        //        gfx.DrawString(expense.ExpenseID.ToString(), textFont, XBrushes.Black, new XPoint(tableStartX + 420, currentY + 17));

        //        // Generate and draw QR Code
        //        var qrCodeImage = GenerateQRCode(expense.ExpenseID.ToString(), 100); // Generate QR Code of size 100x100
        //        gfx.DrawImage(qrCodeImage, tableStartX + 500, currentY); // Position the QR Code

        //        currentY += rowHeight;
        //        if (currentY + rowHeight > pdfPage.Height - 100)
        //        {
        //            gfx.DrawString("© 2024 Expense Tracker - All Rights Reserved", footerFont, XBrushes.Gray, new XPoint(pdfPage.Width / 2, pdfPage.Height - 30), XStringFormats.Center);
        //            pdfPage = pdfDocument.AddPage();
        //            gfx = XGraphics.FromPdfPage(pdfPage);
        //            currentY = 40;
        //        }
        //    }

        //    gfx.DrawString("© 2024 Expense Tracker - All Rights Reserved", footerFont, XBrushes.Gray, new XPoint(pdfPage.Width / 2, pdfPage.Height - 30), XStringFormats.Center);
        //    pdfDocument.Save(pdfFilePath);

        //    return pdfFilePath;
        //}

        //// Helper method to generate QR code
        //private XImage GenerateQRCode(string data, int size)
        //{
        //    using (var qrGenerator = new ZXing.BarcodeWriter { Format = ZXing.BarcodeFormat.QR_CODE, Options = new ZXing.Common.EncodingOptions { Width = size, Height = size } })
        //    {
        //        var qrCodeBitmap = qrGenerator.Write(data);  // Generate QR code bitmap
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            qrCodeBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);  // Save to memory stream
        //            return XImage.FromStream(memoryStream);  // Convert to XImage
        //        }
        //    }
        //}



    }


}


