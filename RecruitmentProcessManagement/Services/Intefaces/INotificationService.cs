using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Services.Intefaces
{
    public interface INotificationService
    {
        Task SendInterviewNotification(int candidateId, string message);
        Task<List<Notification>> GetUserNotifications(int userId);
    }
}
