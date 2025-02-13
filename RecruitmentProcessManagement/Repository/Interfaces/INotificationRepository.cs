using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task SendInterviewNotification(int candidateId, string message);
        Task<List<Notification>> GetUserNotifications(int userId);
    }
}
