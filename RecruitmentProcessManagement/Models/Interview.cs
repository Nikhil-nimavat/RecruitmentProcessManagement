using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class Interview
    {
        public int InterviewID { get; set; }
        public int CandidateID { get; set; }
        public int PositionID { get; set; }
        public string InterviewType { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewerID { get; set; }  // IdentityUser ID ()
        public string Status { get; set; }
        public string Feedback { get; set; }

        // Navgational Properties

        public Candidate Candidate { get; set; }
        public Position Position { get; set; }
        public IdentityUser Interviewer { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
