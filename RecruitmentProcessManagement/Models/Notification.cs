using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class Notification
    {
        [Key]
        public int NotificationID { get; set; }
        public string UserID { get; set; }  // IdentityUser ID ()
        public string Message { get; set; }
        public bool IsRead { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navgation property
        public IdentityUser User { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
