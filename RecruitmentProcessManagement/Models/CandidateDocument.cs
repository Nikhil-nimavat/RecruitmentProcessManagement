namespace RecruitmentProcessManagement.Models
{
    public class CandidateDocument
    {
        public int DocumentID { get; set; }
        public int CandidateID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public string VerificationStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation property
        public Candidate Candidate { get; set; }
    }
}
