using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class Position
    {
        [Key]
        public int PositionID { get; set; }

        [Required (ErrorMessage = "Job Title is Required")]
        public string JobTitle { get; set; }

        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Job Description should have atleast 10 characters.")]
        public string JobDescription { get; set; }

        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Minimum Required Skills should have atleast 10 characters.")]
        public string MinRequiredSkills { get; set; }

        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Job Description should have atleast 10 characters.")]
        public string PreferredSkills { get; set; }

        [Range(0, 1000, ErrorMessage = "Years Of Experience can not be nagative.")]
        public int YearsOfExperience { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
        public string? ReasonForClosure { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PositionClosedDate { get; set; }
        public int? LinkedCandidateID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        // Navigation property
        public Candidate? LinkedCandidate { get; set; }
        public ICollection<PositionSkill>? PositionSkills { get; set; }
        public ICollection<CandidateReview>? CandidateReviews { get; set; }
        public ICollection<Interview>? Interviews { get; set; }
        public ICollection<FinalSelection>? FinalSelections { get; set; }
    }
}
