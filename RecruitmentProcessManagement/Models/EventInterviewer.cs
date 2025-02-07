using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class EventInterviewer
    {
        [Key]
        public int EventInterviewerID { get; set; }
        public int EventID { get; set; }
        public int InterviewerID { get; set; }

        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public Event Event { get; set; }
        public Interviewer Interviewer { get; set; }
    }
}
