using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class Skill
    {
        [Key]
        public int SkillID { get; set; }

        [Required]
        public string SkillName { get; set; }

        // Navigation property
        public ICollection<PositionSkill> PositionSkills { get; set; }
        public ICollection<CandidateSkill> CandidateSkills { get; set; }
    }

}
