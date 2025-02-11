using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class Interviewer
    {
        [Key]
        public int InterviewerID { get; set; }
        public string FullName { get; set; } 
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Designation { get; set; }
        public string? ExperienceYears { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } 

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedDate { get; set; }

        // Navigation Property
        public ICollection<InterviewerSkill> InterviewerSkills { get; set; }
    }
}
