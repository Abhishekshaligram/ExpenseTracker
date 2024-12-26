namespace PracticeCrud.Common.EmailNotification
{
    public class EmailSent
    {
        private IConfiguration _configuration;
        public EmailSent(IConfiguration configuration)
        {
            _configuration = configuration; // Initialization of _configuration
        }

        private EmailSetting GetEmailSetting()
        {
            var MailSettings = _configuration.GetSection("SMTPSettings");
            EmailSetting EmailSetting = new()
            {
                EmailPort = MailSettings.GetValue<int>("EmailPort"),
                EmailUsername = MailSettings.GetValue<string>("EmailUsername") ?? string.Empty,
                EmailPassword = MailSettings.GetValue<string>("EmailPassword") ?? string.Empty,
                EmailEnableSsl = MailSettings.GetValue<bool>("EmailEnableSsl"),
                FromEmail = MailSettings.GetValue<string>("FromEmail") ?? string.Empty,
                FromName = MailSettings.GetValue<string>("FromName") ?? string.Empty,
                EmailHostName = MailSettings.GetValue<string>("EmailHostName") ?? string.Empty,
                EmailAppPassword = MailSettings.GetValue<string>("EmailAppPassword") ?? string.Empty
            };
            return EmailSetting;
        }
    }
}
