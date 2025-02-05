using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class InterviewFeedback
    {
        [Key]
        public int FeedbackID { get; set; }
        public int InterviewRoundID { get; set; }
        public string InterviewerID { get; set; }  // IdentityUser ID ()
        public string FeedbackText { get; set; }
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigational Properties
        public InterviewRound InterviewRound { get; set; }
        public IdentityUser Interviewer { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
