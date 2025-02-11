using System;
using System.Collections.Generic;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class InterviewDetailsViewModel
    {
        public int InterviewId { get; set; }
        public string CandidateName { get; set; }
        public string PositionTitle { get; set; }
        public string InterviewType { get; set; }
        public DateTime InterviewDate { get; set; }
        public string Status { get; set; }
        public List<string> Interviewers { get; set; }
        public string Feedback { get; set; }
    }
}
