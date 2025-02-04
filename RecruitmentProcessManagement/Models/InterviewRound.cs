namespace RecruitmentProcessManagement.Models
{
    public class InterviewRound
    {
        public int InterviewRoundID { get; set; }
        public int InterviewID { get; set; }
        public int RoundNumber { get; set; }
        public string RoundType { get; set; }
        public string Feedback { get; set; }
        public int Rating { get; set; }

        // Navgational Property
        public Interview Interview { get; set; }
    }
}
