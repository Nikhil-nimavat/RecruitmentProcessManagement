using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class CandidateDocument
    {
        [Key]
        public int DocumentID { get; set; }
        public int CandidateID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public string VerificationStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property
        public Candidate Candidate { get; set; }
    }
}
