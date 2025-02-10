using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class CandidateSkillViewModel
    {
        public int SkillID { get; set; }

        [Required(ErrorMessage = "Please enter years of experience.")]
        [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50.")]
        public int YearsOfExperience { get; set; }
    }
}
