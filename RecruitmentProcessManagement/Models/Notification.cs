using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string UserID { get; set; }  // IdentityUser ID ()
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navgation property
        public IdentityUser User { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
