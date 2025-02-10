using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class CandidateSkill
    {
        [Key]
        public int CandidateSkillID { get; set; }
        public string CandidateID { get; set; }
        public int SkillID { get; set; }
        public int YearsOfExperience { get; set; }

        // Navigation properties
        public Candidate Candidate { get; set; }
        public Skill Skill { get; set; }
    }
}
