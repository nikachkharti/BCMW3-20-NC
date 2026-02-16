using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Quartz;

namespace Forum.Application.Jobs
{
    public class UnlockNotificationJob(INotificationService notificationService, IUserRepository userRepository) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var unlockedUsers = await userRepository.GetUnlockedUsers();

            if (unlockedUsers.Count > 0)
            {
                foreach (var user in unlockedUsers)
                {
                    await userRepository.UnlockAsync(user);
                    //await notificationService.SendAsync(); // TODO Implement
                }
            }
        }
    }
}
