using System;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class ManageInterviewsViewModel
    {
        public int InterviewId { get; set; }
        public string CandidateName { get; set; }
        public string PositionTitle { get; set; }
        public DateTime InterviewDate { get; set; }
        public string InterviewType { get; set; }
        public string Status { get; set; }
    }
}
