using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class CandidateStatusHistory
    {
        public int StatusHistoryID { get; set; }
        public int CandidateID { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public string ChangedBy { get; set; }  // IdentityUser ID ()
        public DateTime ChangedDate { get; set; }

        // Navgational properties
        public Candidate Candidate { get; set; }
        public IdentityUser ChangedByUser { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
