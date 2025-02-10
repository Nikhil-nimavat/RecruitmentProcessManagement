using System.ComponentModel.DataAnnotations;
using RecruitmentProcessManagement.Models;

namespace RecruitmentProcessManagement.Models
{
    public class InterviewRound
    {
        [Key]
        public int InterviewRoundID { get; set; }
        public int InterviewID { get; set; }
        public int RoundNumber { get; set; }
        public string RoundType { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navgational Property
        public Interview Interview { get; set; }
        public ICollection<InterviewRoundInterviewer> InterviewRoundInterviewers { get; set; }
    }
}