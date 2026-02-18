using Forum.Application.Models.Notification;

namespace Forum.Application.Contracts.Service
{
    public interface INotificationService
    {
        Task<SendEmailResponse> SendAsync(string to, string subject, string body);
    }
}
