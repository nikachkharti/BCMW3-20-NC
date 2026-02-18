namespace Forum.Application.Models.Notification
{
    public record SendEmailResponse(bool success, string message, Exception error = null);
}
