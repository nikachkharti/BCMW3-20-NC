using System.Runtime.InteropServices;

namespace WebApiFirst.Services
{
    public interface INotificationService
    {
        public Guid InstanceId { get; }
        public void Send();
    }

    public class EmailService : INotificationService
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
        public void Send() => Console.WriteLine($"Email notification sent | {InstanceId}");
    }

    public class SmsService : INotificationService
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
        public void Send() => Console.WriteLine($"SMS notification sent | {InstanceId}");
    }
}
