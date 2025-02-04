using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
   public class CandidateReview
    {
        public int ReviewID { get; set; }
        public int CandidateID { get; set; }
        public int PositionID { get; set; }
        public string ReviewerID { get; set; }  // IdentityUser ID ()
        public DateTime ReviewDate { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }

        // Navigaional Property
        public Candidate Candidate { get; set; }
        public Position Position { get; set; }
        public IdentityUser Reviewer { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
