using System;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class ManageInterviewsViewModel
    {
        public int InterviewId { get; set; }
        public string CandidateName { get; set; }
        public string PositionTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTime InterviewDate { get; set; }
        public string InterviewType { get; set; }
        public string Status { get; set; }

        public int RoundNumber { get; set; }
    }
}
