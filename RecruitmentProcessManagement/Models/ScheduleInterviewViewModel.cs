using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class ScheduleInterviewViewModel
    {
        [Required]
        public int CandidateId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public string InterviewType { get; set; }

        [Required]
        public DateTime InterviewDate { get; set; }

        [Required]
        public List<string> InterviewerIds { get; set; }
    }

}
