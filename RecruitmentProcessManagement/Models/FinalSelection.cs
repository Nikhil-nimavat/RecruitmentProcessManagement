namespace RecruitmentProcessManagement.Models
{
    public class FinalSelection
    {
        public int SelectionID { get; set; }
        public int CandidateID { get; set; }
        public int PositionID { get; set; }
        public string OfferLetterPath { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Status { get; set; }

        // Navigational Properties.
        public Candidate Candidate { get; set; }
        public Position Position { get; set; }
    }
}
