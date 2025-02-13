using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;
using RecruitmentProcessManagement.Services.Intefaces;

namespace RecruitmentProcessManagement.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;

        public NotificationService(INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task SendInterviewNotification(int candidateId, string message)
        {
            await _repository.SendInterviewNotification(candidateId, message);
        }

        public async Task<List<Notification>> GetUserNotifications(int userId)
        {
            return await _repository.GetUserNotifications(userId);
        }
    }
}
