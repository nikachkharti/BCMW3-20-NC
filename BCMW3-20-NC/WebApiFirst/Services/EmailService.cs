namespace WebApiFirst.Services
{
    public interface INotificationService
    {
        void Send();
        public Guid Id { get; }
    }


    public class EmailService : INotificationService
    {
        public Guid Id { get; } = Guid.NewGuid();

        public void Send() => Console.WriteLine($"Email notification sent | Instance: {Id}");
    }

    public class SmsService : INotificationService
    {
        public Guid Id { get; } = Guid.NewGuid();

        public void Send() => Console.WriteLine($"SMS notification sent | Instance: {Id}");
    }
}
