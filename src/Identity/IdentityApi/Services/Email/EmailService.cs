
using SendGrid;
using SendGrid.Helpers.Mail;
using static IdentityApi.Global.GlobalConstant;

namespace IdentityApi.Services.Email
{

    public class EmailService : IEmailService
    {
        #region feilds
        public ILogger<EmailService> Logger { get; }

        #endregion

        #region ctor
        public EmailService(ILogger<EmailService> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }
        #endregion

        #region methods
        public async Task<bool> SendEmail(EmailModel email)
        {
            var apiKey = EmailAccount.SendGrid.apiKey;
            var client = new SendGridClient(apiKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var emailBody = email.Body;

            var from = new EmailAddress
            {
                Email = EmailAccount.SendGrid.SendFromEmail,
                Name = EmailAccount.SendGrid.SendFromAs
            };
            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(sendGridMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Logger.LogInformation($"--> {response.StatusCode}: Email sent to '{to.Email}'");
                return true;

            }

            Logger.LogError($"--> {response.StatusCode}: Email sending failed.");
            return false;
        }

        public async Task<bool> SendEmailBySendGrid(string SendToEmail, string Subject, string Body, string SendToName = "Customer")
        {

            try
            {
                var apiKey = EmailAccount.SendGrid.apiKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(EmailAccount.SendGrid.SendFromEmail, EmailAccount.SendGrid.SendFromAs);
                var subject = Subject;
                var sendToEmail = SecurityProvider.DecryptTextAsync(SendToEmail);
                var to1 = new EmailAddress(sendToEmail, SendToName);
                var to2 = new EmailAddress("tauqirk65@gmail.com", SendToName);
                List<EmailAddress> to = new List<EmailAddress>();
                to.Add(to1);
                to.Add(to2);
                var plainTextContent = Body;
                var htmlContent = Body;
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, to, subject, plainTextContent, htmlContent, true);
                //var msg = MailHelper.CreateSingleEmail(from, to1, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                if (response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Logger.LogInformation($"--> {response.StatusCode}: Email sent to '{sendToEmail}'");
                    return true;
                }

                Logger.LogError($"--> {response.StatusCode}: Email sending failed.");
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
