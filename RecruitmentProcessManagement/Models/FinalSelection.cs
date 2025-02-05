using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models
{
    public class FinalSelection
    {
        [Key]
        public int SelectionID { get; set; }
        public int CandidateID { get; set; }
        public int PositionID { get; set; }
        public string OfferLetterPath { get; set; }

        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }
        public string Status { get; set; }

        // Navigational Properties.
        public Candidate Candidate { get; set; }
        public Position Position { get; set; }
    }
}
