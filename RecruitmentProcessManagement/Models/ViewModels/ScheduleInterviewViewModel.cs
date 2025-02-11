using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentProcessManagement.Models.ViewModels
{
    public class ScheduleInterviewViewModel
    {
        public int InterviewId { get; set; }

        [Required]
        public int CandidateId { get; set; }
        public List<SelectListItem> Candidates { get; set; } = new List<SelectListItem>();

        [Required]
        public int PositionId { get; set; }
        public List<SelectListItem> Positions { get; set; } = new List<SelectListItem>();

        [Required]
        public string InterviewType { get; set; }

        [Required]
        public DateTime InterviewDate { get; set; }

        [Required]
        public List<string> InterviewerIds { get; set; } = new List<string>();
        public List<SelectListItem> Interviewers { get; set; } = new List<SelectListItem>();
    }
}
