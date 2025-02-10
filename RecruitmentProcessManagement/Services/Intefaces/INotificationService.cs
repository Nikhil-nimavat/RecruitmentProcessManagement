using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface INotificationService
    {
        Task SendInterviewNotification(string candidateId, string message);
        Task<List<Notification>> GetUserNotifications(string userId);
    }
}
