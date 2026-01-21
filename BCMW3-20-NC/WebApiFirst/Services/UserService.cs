namespace WebApiFirst.Services
{
    public class UserService
    {
        private readonly INotificationService _notificationService;
        public UserService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Register()
        {
            _notificationService.Send();
        }
    }
}
