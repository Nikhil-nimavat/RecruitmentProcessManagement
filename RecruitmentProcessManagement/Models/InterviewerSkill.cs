using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class InterviewerSkill
    {
        [Key]
        public int InterviewerSkillID { get; set; }
        public int InterviewerID { get; set; } // Reference to Interviewer
        public int SkillID { get; set; } // Reference to Skill
        public string? YearsOfExperience { get; set; }

        // Navigation Property
        public Interviewer Interviewer { get; set; }
        public Skill Skill { get; set; }
    }
}
