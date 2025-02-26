using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecruitmentProcessManagement.Models
{
    public class CandidateReviewViewModel
    {
        public int CandidateID { get; set; }
        public string CandidateName { get; set; }

        public int PositionID { get; set; }
        public string JobTitle { get; set; }

        public string ReviewerID { get; set; }

        [Required(ErrorMessage = "Comments are required.")]
        [StringLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters.")]
        public string Comments { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        public string Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? OfferLetterDate { get; set; }

        //public Candidate candidate { get; set; }

        public List<CandidateSkillViewModel> CandidateSkills { get; set; }
        public SelectList SkillsList { get; set; }
    }
}
