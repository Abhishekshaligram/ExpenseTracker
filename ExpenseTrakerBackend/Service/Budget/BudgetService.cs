using Microsoft.Extensions.Options;
using PracticeCrud.Common.EmailNotification;
using PracticeCrud.Common.JwtAuthentication;
using PracticeCrud.Common.Settings;
using PracticeCrud.Model;
using PracticeCrud.Model.Budget;
using PracticeCrud.Repository.Budget;
using PracticeCrud.Repository.Category;
using System.Reflection;
using PracticeCrud.Common.CommonMethods;
using System.Globalization;

namespace PracticeCrud.Service.Budget
{
    public class BudgetService :IBudgetService
    {
        private readonly IConfiguration _configuration;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        private readonly SmtpSettings _smtpSettings;
        public BudgetService(IConfiguration configuration, IBudgetRepository budgetRepository, IHttpContextAccessor httpContextAccessor,
            IJwtAuthenticationService jwtAuthenticationService, IOptions<SmtpSettings> smtpSettings)
        {
            _configuration= configuration;
            _budgetRepository= budgetRepository;
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthenticationService = jwtAuthenticationService;
            _smtpSettings = smtpSettings.Value;
        }
        public async Task<ResponseModel> AddEditBudget(BudgetreqModel model)
        {
            return await _budgetRepository.AddEditBudget(model);
        }

        public async  Task<ResponseModel> DeleteBudgetById(int budgetID)
        {
            return await _budgetRepository.DeleteBudgetById(budgetID);
        }

        public async Task<List<BudgetListResponseModel>> GetbudgetList(BudgetListReqModel model)
        {
            return await _budgetRepository.GetbudgetList(model);
        }

        public async Task<List<ExpenseCardResponseModel>> GetExpeneForCard(long userId)
        {
            var expenseCards = await _budgetRepository.GetExpeneForCard(userId);

            if (expenseCards != null && expenseCards.Any())
            {
                var lowBudgetCards = expenseCards.Where(card => card.RemainingBudget < 1000).ToList();

                foreach (var card in lowBudgetCards)
                {
                    // Skip sending email if TotalExpenses and TotalBudget are both 0
                    if (card.TotalExpenses == 0 && card.TotalBudget == 0)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(card.Email) && !string.IsNullOrEmpty(card.userName))
                    {
                        bool isMailSent = await SendLowBudgetEmailAsync(card.Email, card.userName, new List<ExpenseCardResponseModel> { card });

                        if (!isMailSent)
                        {
                            Console.WriteLine($"Failed to send email to {card.Email}");
                        }
                    }
                }
            }

            return expenseCards;
        }

        private async Task<bool> SendLowBudgetEmailAsync(string recipientEmail, string userName, List<ExpenseCardResponseModel> lowBudgetCards)
        {
            try
            {
                string emailBody;
                string budgetRows = "";
                string BasePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.EmailTemplate);
                if (!Directory.Exists(BasePath))
                {
                    Directory.CreateDirectory(BasePath);
                }
                using (StreamReader reader = new StreamReader(Path.Combine(BasePath, Constants.budgetLowMail)))
                {
                    emailBody = reader.ReadToEnd();
                }

                var cultureInfo = new CultureInfo("hi-IN"); 
                foreach (var card in lowBudgetCards)
                {
                    budgetRows += $"<tr><td>{card.TotalBudget.ToString("C", cultureInfo)}</td>" +
                                  $"<td>{card.TotalExpenses.ToString("C", cultureInfo)}</td>" +
                                  $"<td>{card.RemainingBudget.ToString("C", cultureInfo)}</td></tr>";
                }

                string path = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host.Value;
                emailBody = emailBody.Replace("##userName##", userName)
                                     .Replace("##BudgetRows##", budgetRows)
                                     .Replace("##FooterLogoURL##", path + _configuration["Images:FooterLogo"])
                                     .Replace("##currentYear##", DateTime.Now.Year.ToString());

                EmailSetting setting = new EmailSetting
                {
                    EmailEnableSsl = Convert.ToBoolean(_smtpSettings.EmailEnableSsl),
                    EmailHostName = _smtpSettings.EmailHostName,
                    EmailPassword = _smtpSettings.EmailPassword,
                    EmailAppPassword = _smtpSettings.EmailAppPassword,
                    EmailPort = Convert.ToInt32(_smtpSettings.EmailPort),
                    FromEmail = _smtpSettings.FromEmail,
                    FromName = _smtpSettings.FromName,
                    EmailUsername = _smtpSettings.EmailUsername,
                };

                return await Task.Run(() => EmailNotifications.SendMailMessage(recipientEmail, null, null, "Low Budget Alert", emailBody, setting, null));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }


    }
}
