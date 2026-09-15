using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using OpenLedger.Application.Interfaces.Services;
using OpenLedger.Application.Options;

namespace OpenLedger.Infrastructure.Services
{
    public class EmailService(IOptions<EmailOptions> emailOptions) : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(emailOptions.Value.FromName, emailOptions.Value.FromEmail!));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            var socketOptions = emailOptions.Value.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.Auto;

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(emailOptions.Value.SmtpServer!, emailOptions.Value.SmtpPort, socketOptions, cancellationToken);

            if (!string.IsNullOrEmpty(emailOptions.Value.SmtpUsername) && !string.IsNullOrEmpty(emailOptions.Value.SmtpPassword))
            {
                await smtp.AuthenticateAsync(emailOptions.Value.SmtpUsername, emailOptions.Value.SmtpPassword, cancellationToken);
            }

            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
