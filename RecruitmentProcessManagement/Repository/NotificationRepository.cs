using Microsoft.EntityFrameworkCore;
using RecruitmentProcessManagement.Data;
using RecruitmentProcessManagement.Models;
using RecruitmentProcessManagement.Repository.Interfaces;

namespace RecruitmentProcessManagement.Repository
{
    public class NotificationRepository: INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendInterviewNotification(int candidateId, string message)
        {
            var notification = new Notification
            {
                UserID = candidateId.ToString(),
                Message = message,
                CreatedDate = DateTime.Now
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notification>> GetUserNotifications(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserID == userId.ToString())
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }
    }
}
