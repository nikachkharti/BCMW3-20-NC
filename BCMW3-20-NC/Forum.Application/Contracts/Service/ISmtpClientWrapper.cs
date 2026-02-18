using MimeKit;

namespace Forum.Application.Contracts.Service
{
    public interface ISmtpClientWrapper : IDisposable
    {
        Task ConnectAsync(string host, int port, bool useSsl);
        Task AuthenticateAsync(string userName, string password);
        Task SendAsync(MimeMessage message);
        Task DisconnectAsync(bool quit);
    }
}
