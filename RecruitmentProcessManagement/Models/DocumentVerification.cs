using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class DocumentVerification
    {
        [Key]
        public int VerificationID { get; set; }
        public int? CandidateID { get; set; }
        public string VerificationStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime VerificationDate { get; set; }
        public string VerifiedBy { get; set; }  // IdentityUser ID ()

        // Navgational properties
        public Candidate Candidate { get; set; }
        public IdentityUser VerifiedByUser { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
