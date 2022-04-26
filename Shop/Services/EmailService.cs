using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using Shop.Options;
using MailKit.Net.Smtp;

namespace Shop.Services
{
    public class EmailService : IEmailSender
    {
        public EmailOptions Options { get; set; }
        public EmailService(IOptions<EmailOptions> options)
        {
            Options = options.Value;
        }
        public Task SendEmailAsync(string to, string subject, string message)
        {
            return Execute(to, subject, message);
        }
        public async Task Execute(string to, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(Options.Email));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = message };

            using var smtp = new SmtpClient();
            smtp.Connect(Options.Host, Options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(Options.Email, Options.Password);
            await smtp.SendAsync(email);
        }
    }
}
