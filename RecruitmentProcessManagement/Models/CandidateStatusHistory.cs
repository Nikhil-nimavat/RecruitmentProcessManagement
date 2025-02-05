using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class CandidateStatusHistory
    {
        [Key]
        public int StatusHistoryID { get; set; }
        public int CandidateID { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public string ChangedBy { get; set; }  // IdentityUser ID ()

        [DataType(DataType.Date)]
        public DateTime ChangedDate { get; set; } = DateTime.Now;

        // Navgational properties
        public Candidate Candidate { get; set; }
        public IdentityUser ChangedByUser { get; set; }  // Navigation to AspNetUser (Identity Table)
    }
}
