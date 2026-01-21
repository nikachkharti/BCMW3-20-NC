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


    public class TeacherService
    {
        private readonly INotificationService _notificationService;
        public TeacherService(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Register()
        {
            _notificationService.Send();
        }
    }







}
