using Forum.Application.Contracts.Service;
using MimeKit;
using MailKit.Net.Smtp;

namespace Forum.Application.Models.Notification
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        private readonly SmtpClient _client = new();

        public async Task AuthenticateAsync(string userName, string password) => await _client.AuthenticateAsync(userName, password);
        public async Task ConnectAsync(string host, int port, bool useSsl) => await _client.ConnectAsync(host, port, useSsl);
        public async Task DisconnectAsync(bool quit) => await _client.DisconnectAsync(quit);
        public async Task SendAsync(MimeMessage message) => await _client.SendAsync(message);
        public void Dispose() => _client.Dispose();
    }
}
