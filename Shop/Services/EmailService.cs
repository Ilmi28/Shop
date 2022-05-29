using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Shop.Services
{
    public class EmailService : IEmailSender
    {
        private readonly KeyVaultService _keyVault;
        public EmailService(KeyVaultService keyVault)
        {
            _keyVault = keyVault;
        }
        public Task SendEmailAsync(string to, string subject, string message)
        {
            return Execute(to, subject, message);
        }
        public async Task Execute(string to, string subject, string message)
        {
            var apiKey = _keyVault.GetSecret("SendGridAPI");
            var from = _keyVault.GetSecret("SendGridEmail");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(from),
                Subject = subject,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(to));
            await client.SendEmailAsync(msg);
        }
    }
}
