using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class Interview
    {
        [Key]
        public int InterviewID { get; set; }
        public int CandidateID { get; set; }
        public int PositionID { get; set; }
        public int InterviewerID { get; set; }
        public string InterviewType { get; set; }

        [DataType(DataType.Date)]
        public DateTime InterviewDate { get; set; }
        public string Status { get; set; }
        public string? Feedback { get; set; }

        // Navgational Properties

        public Candidate Candidate { get; set; }
        public Position Position { get; set; }
        public ICollection<InterviewRound> InterviewRounds { get; set; }

    }
}
