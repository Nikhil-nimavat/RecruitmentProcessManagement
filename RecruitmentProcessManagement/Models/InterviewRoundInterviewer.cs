using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class InterviewRoundInterviewer
    {
        [Key]
        public int RoundInterviewerID { get; set; }
        public int InterviewRoundID { get; set; } // ref to round 
        public int InterviewerID { get; set; } // ref to interviewer
        public string? Feedback { get; set; }
        public int? Rating { get; set; }

        // Navigational Properties
        public InterviewRound InterviewRound { get; set; }
        public Interviewer Interviewer { get; set; }
    }
}
