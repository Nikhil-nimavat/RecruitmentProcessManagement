using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class Candidate
    {
        [Key]
        public int CandidateID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ResumePath { get; set; }
        public string ProfileStatus { get; set; }
        public DateTime CreatedDate { get; set; }
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
