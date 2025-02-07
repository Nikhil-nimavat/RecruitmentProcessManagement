using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RecruitmentProcessManagement.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        public string Location { get; set; } 
        public int EventOrganizerID { get; set; } 
        public int TotalParticipants { get; set; } 
        public string EventStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;  

        [DataType(DataType.Date)]
        public DateTime? ModifiedDate { get; set; }

        // Navigaiton Property
        public IdentityUser EventOrganizer { get; set; }
        public List<EventCandidate> EventCandidates { get; set; }
        public List<EventInterviewer> EventInterviewers { get; set; }
    }
}
