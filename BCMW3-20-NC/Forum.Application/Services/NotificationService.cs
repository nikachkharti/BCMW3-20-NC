using Forum.Application.Contracts.Service;
using Forum.Application.Exceptions;
using Forum.Application.Models.Notification;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using Serilog;
using System.Net.Mail;

namespace Forum.Application.Services
{
    public class NotificationService(IConfiguration configuration, ISmtpClientWrapper smtpClient) : INotificationService
    {
        public async Task<SendEmailResponse> SendAsync(string to, string subject, string body)
        {
            try
            {
                Log.Information("Starting to send email to {Recepient}", to);
                ValidateAddressWhereEmailSent(to);

                var normalizedSubject = NormalizeSubject(subject);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(configuration["EmailSettings:Sender"]));
                email.To.Add(MailboxAddress.Parse(to.Trim()));
                email.Subject = normalizedSubject;
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                Log.Information("Connecting to SMTP server: {Server}:{Port}", configuration["EmailSettings:SmtpServer"], configuration["EmailSettings:Port"]);

                await smtpClient.ConnectAsync(
                    configuration["EmailSettings:SmtpServer"],
                    int.Parse(configuration["EmailSettings:Port"]),
                    bool.Parse(configuration["EmailSettings:UseSsl"])
                );

                Log.Information("Authenticating with SMTP server...");

                await smtpClient.AuthenticateAsync(
                    configuration["EmailSettings:Username"],
                    configuration["EmailSettings:Password"]
                );

                Log.Information("Sending email to {Recipient}", to);
                await smtpClient.SendAsync(email);

                Log.Information("Disconnecting from SMTP server...");
                await smtpClient.DisconnectAsync(true);

                Log.Information("Email sent successfully to {Recipient}", to);

                return new SendEmailResponse(true, $"Message sent successfully to: {to}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to send email to {Recepient}: {Message}", to, ex.Message);
                return new SendEmailResponse(false, ex.Message, ex);
            }
        }



        private string NormalizeSubject(string subject) => string.IsNullOrWhiteSpace(subject) ? string.Empty : subject;

        private void ValidateAddressWhereEmailSent(string to)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new BadRequestException("Email address can't be empty");

            var mailAddress = new MailAddress(to);
            if (!mailAddress.Address.Contains("@") || !mailAddress.Address.Contains("."))
                throw new BadRequestException("Invalid email addres format");
        }


    }
}
