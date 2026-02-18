using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Quartz;

namespace Forum.Application.Jobs
{
    public class UnlockNotificationJob : IJob
    {
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepository;

        public UnlockNotificationJob(INotificationService notificationService, IUserRepository userRepository)
        {
            _notificationService = notificationService;
            _userRepository = userRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var unlockedUsers = await _userRepository.GetUnlockedUsers();

            if (unlockedUsers.Count > 0)
            {
                foreach (var user in unlockedUsers)
                {
                    await _userRepository.UnlockAsync(user);
                    await _notificationService.SendAsync(user.Email, "Account Unlock", "<h1>Your Account Unlocked Successfully</h1>");
                }
            }
        }
    }
}
