using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateID { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
        public string? ResumePath { get; set; }

        [MaxLength(50)]
        public string? ProfileStatus { get; set; }
        public string? CollegeName { get; set; }
        public string? ExtractedText { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        // Nav Property
        public ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
        public ICollection<CandidateDocument> CandidateDocuments { get; set; } = new List<CandidateDocument>();
        public ICollection<CandidateReview> CandidateReviews { get; set; } = new List<CandidateReview>();
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
        public ICollection<FinalSelection> FinalSelections { get; set; } = new List<FinalSelection>();
        public ICollection<CandidateStatusHistory> CandidateStatusHistories { get; set; } = new List<CandidateStatusHistory>();
        public ICollection<DocumentVerification> DocumentVerifications { get; set; } = new List<DocumentVerification>();
    }
}
