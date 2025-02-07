using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class EventCandidate
    {
        [Key]
        public int EventCandidateID { get; set; }
        public int EventID { get; set; }
        public int CandidateID { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string AttendanceStatus { get; set; } // maybe a enum or text for reason

        // nav property
        public Event Event { get; set; }
        public Candidate Candidate { get; set; }
    }
}
