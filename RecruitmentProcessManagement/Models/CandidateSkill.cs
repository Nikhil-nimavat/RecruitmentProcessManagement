namespace RecruitmentProcessManagement.Models
{
    public class CandidateSkill
    {
        public int CandidateSkillID { get; set; }
        public int CandidateID { get; set; }
        public int SkillID { get; set; }
        public int YearsOfExperience { get; set; }

        // Navigation properties
        public Candidate Candidate { get; set; }
        public Skill Skill { get; set; }
    }
}
