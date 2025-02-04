using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class DocumentVerification
    {
        public int VerificationID { get; set; }
        public int CandidateID { get; set; }
        public string VerificationStatus { get; set; }
        public DateTime VerificationDate { get; set; }
        public string VerifiedBy { get; set; }  // IdentityUser ID ()

        // Navgational properties
        public Candidate Candidate { get; set; }
        public IdentityUser VerifiedByUser { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
