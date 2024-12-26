﻿using System.Net.Mail;
using System.Net.Mime;

namespace PracticeCrud.Common.EmailNotification
{
    public class EmailNotifications
    {

        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        /// <param name="bcc">The BCC.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="emailSetting">The email setting.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SendMailMessage(string recipient, string bcc, string cc, string subject, string body, EmailSetting emailSetting, string attachment, string? ImagePath = null)
        {
            if (string.IsNullOrEmpty(recipient))
            {
                return false;   
            }

            List<EmailImages> emailImages = new List<EmailImages>
            {
                new EmailImages() { ImageName = !string.IsNullOrEmpty(ImagePath)?ImagePath:"", ContentId = "userphoto",Type="userphoto" },
                new EmailImages() { ImageName = "tom-logo.png", ContentId = "companylogo",Type="logo" },
                new EmailImages() { ImageName = "tom-logo.png", ContentId = "UserAdd",Type="logo" },
            };

            // Instantiate a new instance of MailMessage 
            MailMessage mailMessage = new MailMessage();

            // Set the sender address of the mail message 
            mailMessage.From = new MailAddress(emailSetting.FromEmail ?? "", emailSetting.FromName);


            string[] strRecipient = recipient.Replace(";", ",").TrimEnd(',').Split(new char[] { ',' });

            // Set the Bcc address of the mail message 
            for (int intCount = 0; intCount < strRecipient.Length; intCount++)
            {
                if (strRecipient[intCount] != "null")
                    mailMessage.To.Add(new MailAddress(strRecipient[intCount]));
            }

            // Check if the bcc value is nothing or an empty string 
            if (!string.IsNullOrEmpty(bcc))
            {
                string[] strBCC = bcc.Split(new char[] { ',' });

                // Set the Bcc address of the mail message 
                for (int intCount = 0; intCount < strBCC.Length; intCount++)
                {
                    if (strBCC[intCount] != "null")
                        mailMessage.Bcc.Add(new MailAddress(strBCC[intCount]));
                }
            }

            // Check if the cc value is nothing or an empty value 
            if (!string.IsNullOrEmpty(cc))
            {
                // Set the CC address of the mail message 
                string[] strCC = cc.Split(new char[] { ',' });
                for (int intCount = 0; intCount < strCC.Length; intCount++)
                {
                    if (strCC[intCount] != "null")
                        mailMessage.CC.Add(new MailAddress(strCC[intCount]));
                }
            }

            if (body.Contains("src=\"cid:"))
            {
                string BasePathImages = "";

                AlternateView av1 = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                foreach (var item in emailImages)
                {
                    if (item.Type == "logo")
                    {
                        BasePathImages = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "images");
                    }
                    else
                    {
                        BasePathImages = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", "UserProfile");
                    }

                    if (body.Contains("cid:" + item.ContentId))
                    {
                        if (!string.IsNullOrEmpty(item.ImageName) && item.ImageName != "")
                        {
                            string path = Path.Combine(BasePathImages, item.ImageName);
                            LinkedResource logo = new LinkedResource(path, "image/png");
                            logo.ContentId = item.ContentId;
                            av1.LinkedResources.Add(logo);
                        }
                        else
                        {
                            BasePathImages = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Logo");
                            string path = Path.Combine(BasePathImages, "default_user.png");
                            LinkedResource logo = new LinkedResource(path, "image/png");
                            logo.ContentId = item.ContentId;
                            av1.LinkedResources.Add(logo);
                        }
                    }
                }
                mailMessage.AlternateViews.Add(av1);
            }


            // Set the subject of the mail message 
            mailMessage.Subject = subject;

            // Set the body of the mail message 
            mailMessage.Body = body;

            // Set the format of the mail message body as HTML 
            mailMessage.IsBodyHtml = true;

            // Set the priority of the mail message to normal 
            mailMessage.Priority = MailPriority.Normal;

            // Instantiate a new instance of SmtpClient 
            var smtpClient = new SmtpClient();

            if (attachment != null && attachment != "")
            {
                mailMessage.Attachments.Add(new Attachment(attachment));
            }
            try
            {
                smtpClient.EnableSsl = emailSetting.EmailEnableSsl;
                smtpClient.Host = emailSetting.EmailHostName ?? "";
                smtpClient.Port = emailSetting.EmailPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Credentials = new System.Net.NetworkCredential(emailSetting.EmailUsername, emailSetting.EmailAppPassword);

                // Send the mail message 
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                DisposeOf(mailMessage);
                DisposeOf(smtpClient);
            }
        }

        public static async Task<bool> SendAsyncEmail(string recipient, string bcc, string cc, string subject, string body, EmailSetting emailSetting, string attachment)
        {
            return await Task.Run(() => SendMailMessage(recipient, bcc, cc, subject, body, emailSetting, attachment));
        }



        /// <summary>
        /// Class Email Images.
        /// </summary>


        /// <summary>
        /// Dispose object to release memory
        /// </summary>
        /// <param name="object">The object.</param>
        public static void DisposeOf(object @object)
        {
            if (@object is IDisposable obj)
            {
                obj.Dispose();
            }
        }
    }
    public class EmailImages
    {
        /// <summary>
        /// Gets or sets image name.
        /// </summary>
        /// <value>image name.</value>
        public string? ImageName { get; set; } = "";

        /// <summary>
        /// Gets or sets the name of the image cid.
        /// </summary>
        /// <value>The name of the image cid.</value>
        public string? ContentId { get; set; } = "";
        public string? Type { get; set; } = "";
    }

    public class EmailSetting
    {
        /// <summary>
        /// Gets or sets from email.
        /// </summary>
        /// <value>From email.</value>
        public string? FromEmail { get; set; } = "";

        /// <summary>
        /// Gets or sets the name of the email host.
        /// </summary>
        /// <value>The name of the email host.</value>
        public string? EmailHostName { get; set; } = "";

        /// <summary>
        /// Gets or sets the email port.
        /// </summary>
        /// <value>The email port.</value>
        public int EmailPort { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [email enable SSL].
        /// </summary>
        /// <value><c>true</c> if [email enable SSL]; otherwise, <c>false</c>.</value>
        public bool EmailEnableSsl { get; set; }

        /// <summary>
        /// Gets or sets the email user-name.
        /// </summary>
        /// <value>The email user name.</value>
        public string? EmailUsername { get; set; } = "";

        /// <summary>
        /// Gets or sets the email password.
        /// </summary>
        /// <value>The email password.</value>
        public string? EmailPassword { get; set; } = "";

        /// <summary>
        /// Gets or sets the email app password.
        /// </summary>
        /// <value>The email app password.</value>
        public string? EmailAppPassword { get; set; } = "";

        /// <summary>
        /// Gets or sets from name.
        /// </summary>
        /// <value>From Name</value>
        public string? FromName { get; set; } = "";
    }
}