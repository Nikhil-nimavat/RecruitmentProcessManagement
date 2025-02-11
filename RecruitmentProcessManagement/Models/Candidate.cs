using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateID { get; set; }
        public required string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public string ResumePath { get; set; }

        [MaxLength(50)]
        public string? ProfileStatus { get; set; }
        public string? CollegeName { get; set; }
        public string? ExtractedText { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        // Navigation Property...
        public ICollection<CandidateSkill> CandidateSkills { get; set; }
        public ICollection<CandidateDocument> CandidateDocuments { get; set; }
        public ICollection<CandidateReview> CandidateReviews { get; set; }
        public ICollection<Interview> Interviews { get; set; }
        public ICollection<FinalSelection> FinalSelections { get; set; }
        public ICollection<CandidateStatusHistory> CandidateStatusHistories { get; set; }
        public ICollection<DocumentVerification> DocumentVerifications { get; set; }
    }
}
